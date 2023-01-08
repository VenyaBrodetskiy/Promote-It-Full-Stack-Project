using Tweetinvi.Models.V2;

namespace TwitterAccessor.Services
{
    public class TwitterService
    {
        private readonly TweetinviService _tweetinviService;

        public TwitterService(TweetinviService tweetinviService)
        {
            _tweetinviService = tweetinviService;
        }

        public async Task<List<UserToCampaignTwitterInfo>> CountTweetsForUsers(CampaignInfo campaign, SocialActivistDTO[]? socialActivists)
        {
            // 1. get all tweets by Hashtag
            // 2. sort those which made by users of my platform
            // 3. check if tweets are made by rules: contains link + Hashtag
            // 4. count tweets and retweets for each user

            // 1. get all tweets by Hashtag
            SearchTweetsV2Response searchResponse = await _tweetinviService.userClient.SearchV2.SearchTweetsAsync(campaign.Hashtag);

            // 2. filter posts made by SocialActivists
            List<string>? userTwitterHandles = socialActivists.Select(sa => sa.TwitterHandle).ToList();

            var authors = searchResponse.Includes.Users
                .Where(author => userTwitterHandles.Contains($"@{author.Username}")).ToList();

            var userToCampaigns = new List<UserToCampaignTwitterInfo>();

            authors.ForEach(author =>
            {
                int tweetCount = 0;
                // 3. check if tweets are made by rules: contains link + Hashtag
                // check 1. author 2. that LandingPage is included in tweet message
                var tweetsBySa = searchResponse.Tweets
                    .Where(tweet => tweet.AuthorId == author.Id)
                    .Where(tweet => tweet.Text.IndexOf(campaign.LandingPage) > -1)
                    .ToList();

                // 4. count tweets and retweets for each user
                tweetsBySa
                    .ForEach(tweet => tweetCount += 1 + tweet.PublicMetrics.QuoteCount + tweet.PublicMetrics.RetweetCount);

                Console.WriteLine($"Campaign: {campaign.Hashtag}\nUser: {author.Username}\nValidated tweets + retweets: {tweetCount}");

                userToCampaigns.Add(new UserToCampaignTwitterInfo()
                {
                    UserId = socialActivists.Where(sa => sa.TwitterHandle == $"@{author.Username}").Select(sa => sa.UserId).First(),
                    CampaignId = campaign.CampaignId,
                    CurrentTweetCount = tweetCount,
                });
            });

            return userToCampaigns;

        }
    }
}
