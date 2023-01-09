using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Timers;
using Tweetinvi.Core.Models.Properties;
using Tweetinvi.Models.V2;
using TwitterAccessor.Common;
using Timer = System.Timers.Timer;

namespace TwitterAccessor.Services
{
    public class TwitterService
    {
        private readonly TweetinviService _tweetinviService;
        private readonly ILogger<TwitterService> _logger;
        private readonly HttpClient _httpClient;
        private Timer _timer;

        public TwitterService(TweetinviService tweetinviService, HttpClient httpClient, ILogger<TwitterService> logger)
        {
            _tweetinviService = tweetinviService;
            _logger = logger;
            _httpClient = httpClient;
            _timer = new Timer(100000);
        }

        public void StartPeriodicalCheck(int seconds)
        {
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

                await ChangeUserBalancesForAllCampaigns();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task ChangeUserBalancesForAllCampaigns()
        {
            // 1. get all campaigns
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Const.ApiGetCampaigns);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyRGF0YSI6eyJ1c2VySWQiOjQsInVzZXJUeXBlSWQiOjF9LCJpYXQiOjE2NzMzMDM4MjYsImV4cCI6MTY3MzMxMTAyNn0.ZMV-3dD9ER3zAb8tSszp8Kdom0qJ1erHhJPOK_qLknA");
            HttpResponseMessage campaignsResponse = await _httpClient.SendAsync(request);
            CampaignInfo[]? campaigns = await campaignsResponse.Content.ReadFromJsonAsync<CampaignInfo[]>();

            // 2. get all users (social activists)
            var socialActivistsResponse = await _httpClient.GetAsync(Const.ApiGetSocialActivists);
            SocialActivistDTO[]? socialActivists = await socialActivistsResponse.Content.ReadFromJsonAsync<SocialActivistDTO[]>();

            // 3. for each campaign change user balances
            foreach (var campaign in campaigns)
            {
                await ChangeUserBalancesForCampaign(campaign, socialActivists);
            }
        }

        public async Task ChangeUserBalancesForCampaign(CampaignInfo campaign, SocialActivistDTO[]? socialActivists)
        {
            // 1. get all users (social activists)
            // TODO: move to upper function
            //var socialActivistsResponse = await _httpClient.GetAsync(Const.ApiGetSocialActivists);
            //SocialActivistDTO[]? socialActivists = await socialActivistsResponse.Content.ReadFromJsonAsync<SocialActivistDTO[]>();

            // 2. Count tweets and retweets for each user
            var userToCampaigns = await CountTweetsForUsers(campaign, socialActivists);

            // 3. change balance in DB based on tweet qty
            foreach (var userToCampaign in userToCampaigns)
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(Const.ApiPostUpdateUserBalance, userToCampaign);
                _logger.LogInformation("Status: {status}, Message: {message}", responseMessage.StatusCode, await responseMessage.Content.ReadAsStringAsync());
            };
        }
        private async Task<List<UserToCampaignTwitterInfo>> CountTweetsForUsers(CampaignInfo campaign, SocialActivistDTO[]? socialActivists)
        {
            // 1. get all tweets by Hashtag
            // 2. sort those which made by users of my platform
            // 3. check if tweets are made by rules: contains link + Hashtag
            // 4. count tweets and retweets for each user

            // 1. get all tweets by Hashtag
            SearchTweetsV2Response searchResponse = await _tweetinviService.userClient.SearchV2.SearchTweetsAsync(campaign.Hashtag);
            if (searchResponse.Tweets.Count() == 0) return new List<UserToCampaignTwitterInfo>();

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
                // check 1. author 2. that LandingPage is included in tweet message
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
    }
}
