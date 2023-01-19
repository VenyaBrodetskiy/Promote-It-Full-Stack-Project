using System.Net;
using System.Net.Http.Headers;
using System.Timers;
using Tweetinvi.Models.V2;
using TwitterAccessor.Common;
using TwitterAccessor.Models;
using Timer = System.Timers.Timer;

namespace TwitterAccessor.Services
{
    public class TwitterService
    {
        private readonly TweetinviService _tweetinviService;
        private readonly ILogger<TwitterService> _logger;
        private readonly HttpClient _httpClient;
        private readonly Timer _timer;
        private string? _token { get; set; }

        public TwitterService(TweetinviService tweetinviService, HttpClient httpClient, ILogger<TwitterService> logger)
        {
            _tweetinviService = tweetinviService;
            _logger = logger;
            _httpClient = httpClient;
            _timer = new Timer(100000);
        }

        public void StartPeriodicalCheck(int seconds, string token)
        {
            _token = token;

            double interval = TimeSpan.FromSeconds(seconds).TotalMilliseconds;
            _timer.Interval = interval;
            _timer.Enabled = true;
            _timer.Elapsed += OnTimerEvent;
        }

        public void StopPeriodicalCheck()
        {
            _timer.Enabled = false;
        }

        public async void OnTimerEvent(object? source, ElapsedEventArgs e)
        {
            try
            {
                _logger.LogInformation("Periodical check of twitter started at {0:HH:mm:ss.fff}", e.SignalTime);

                await ChangeUserBalancesForAllCampaigns(_token!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task<List<TweetDTO>> GetAllTweets(string token)
        {
            // 1. get all campaigns
            if (_token == null)
            {
                _token = token;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var campaignsResponse = await _httpClient.GetAsync(EndpointsNodeJs.ApiGetCampaigns);
            if (campaignsResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Not Authorized");
            }

            CampaignInfo[]? campaigns = await campaignsResponse.Content.ReadFromJsonAsync<CampaignInfo[]>();
            if (campaigns == null || campaigns.Length == 0)
            {
                throw new Exception($"Couldn't read social activists from response body.");
            }

            // 2. get all users (social activists)
            var socialActivistsResponse = await _httpClient.GetAsync(EndpointsMain.ApiGetSocialActivists);
            if (!socialActivistsResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Couldn't get Social activists, status: {socialActivistsResponse.StatusCode}");
            }    
            SocialActivistDTO[]? socialActivists = await socialActivistsResponse.Content.ReadFromJsonAsync<SocialActivistDTO[]>();
            if (socialActivists == null)
            {
                throw new Exception($"Couldn't read social activists from response body.");
            }

            var AllTweets = new List<TweetDTO>();
            // 3. for each campaign check tweets by campaign hashtag
            foreach (var campaign in campaigns)
            {
                var tweetsByHashtag = await GetTweetsByHashtag(campaign, socialActivists);
                AllTweets.AddRange(tweetsByHashtag);
            }

            return AllTweets;
        }
        public async Task ChangeUserBalancesForAllCampaigns(string token)
        {
            // 1. get all campaigns
            if (_token == null)
            {
                _token = token;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var campaignsResponse = await _httpClient.GetAsync(EndpointsNodeJs.ApiGetCampaigns);
            if (campaignsResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Not Authorized");
            }

            CampaignInfo[]? campaigns = await campaignsResponse.Content.ReadFromJsonAsync<CampaignInfo[]>();

            // 2. get all users (social activists)
            var socialActivistsResponse = await _httpClient.GetAsync(EndpointsMain.ApiGetSocialActivists);
            SocialActivistDTO[]? socialActivists = await socialActivistsResponse.Content.ReadFromJsonAsync<SocialActivistDTO[]>();

            // 3. for each campaign change user balances
            foreach (var campaign in campaigns)
            {
                await ChangeUserBalancesForCampaign(campaign, socialActivists);
            }
        }

        public async Task ChangeUserBalancesForCampaign(CampaignInfo campaign, SocialActivistDTO[]? socialActivists)
        {
            // 1. Count tweets and retweets for each user
            var userToCampaigns = await CountTweetsForUsers(campaign, socialActivists);

            // 2. change balance in DB based on tweet qty
            foreach (var userToCampaign in userToCampaigns)
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(EndpointsMain.ApiPostUpdateUserBalance, userToCampaign);
                _logger.LogInformation("Status: {status}, Message: {message}", responseMessage.StatusCode, await responseMessage.Content.ReadAsStringAsync());
            };
        }

        private async Task<List<UserToCampaignTwitterInfo>> CountTweetsForUsers(CampaignInfo campaign, SocialActivistDTO[]? socialActivists)
        {
            // 1. get all tweets by Hashtag
            SearchTweetsV2Response searchResponse = await _tweetinviService.userClient.SearchV2.SearchTweetsAsync(campaign.Hashtag);
            if (searchResponse.Tweets.Length == 0) return new List<UserToCampaignTwitterInfo>();

            // 2. filter posts made by SocialActivists
            List<string>? userTwitterHandles = socialActivists.Select(sa => sa.TwitterHandle).ToList();

            var authors = searchResponse.Includes.Users
                .Where(author => userTwitterHandles.Contains($"@{author.Username}")).ToList();

            // move this line up
            var userToCampaigns = new List<UserToCampaignTwitterInfo>();

            authors.ForEach(author =>
            {
                int tweetCount = 0;
                // 3. check if tweets are made by rules: contains link + Hashtag
                var tweetsBySa = searchResponse.Tweets
                    .Where(tweet => tweet.AuthorId == author.Id)
                    .Where(tweet => tweet.Text.IndexOf(campaign.LandingPage) > -1 
                        || tweet.Entities.Urls.FirstOrDefault().ExpandedUrl == campaign.LandingPage)
                    .ToList();

                // 4. count tweets and retweets for each user
                tweetsBySa
                    .ForEach(tweet => tweetCount += 1 + tweet.PublicMetrics.QuoteCount + tweet.PublicMetrics.RetweetCount);

                _logger.LogInformation("Campaign: {hashtag}, User: {username}, Validated tweet + RT: {tweetCount}", campaign.Hashtag, author.Username, tweetCount);

                userToCampaigns.Add(new UserToCampaignTwitterInfo()
                {
                    UserId = socialActivists.Where(sa => sa.TwitterHandle == $"@{author.Username}").Select(sa => sa.UserId).First(),
                    CampaignId = campaign.Id,
                    CurrentTweetCount = tweetCount,
                });
            });

            return userToCampaigns;
        }

        private async Task<List<TweetDTO>> GetTweetsByHashtag(CampaignInfo campaign, SocialActivistDTO[] socialActivists)
        {
            var TweetsByHashtag = new List<TweetDTO>();

            // 1. get all tweets by Hashtag
            SearchTweetsV2Response searchResponse = await _tweetinviService.userClient.SearchV2.SearchTweetsAsync(campaign.Hashtag);
            if (searchResponse.Tweets.Length == 0) return TweetsByHashtag;

            // 2. filter authorsSA ot tweets, who are SocialActivists
            List<string> userTwitterHandles = socialActivists.Select(sa => sa.TwitterHandle).ToList();
            var authorsSA = searchResponse.Includes.Users
                .Where(author => userTwitterHandles.Contains($"@{author.Username}")).ToList();

            // 3. filter tweets made by SocialActivists
            authorsSA.ForEach(author =>
            {
                var tweetsBySa = searchResponse.Tweets
                    .Where(tweet => tweet.AuthorId == author.Id)
                    .Select(tweet => new TweetDTO()
                    {
                        User = author.Username,
                        PublishedOn = tweet.CreatedAt.DateTime,
                        Link = $"https://twitter.com/{author.Username}/status/{tweet.Id}",
                        Retweets = tweet.PublicMetrics.RetweetCount + tweet.PublicMetrics.QuoteCount,
                        Body = tweet.Text
                    }).ToList();
                TweetsByHashtag.AddRange(tweetsBySa);
            });

            return TweetsByHashtag;
        }
    }
}
