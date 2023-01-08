﻿using Tweetinvi.Models.V2;

namespace TwitterAccessor.Services
{
    public class TwitterService
    {
        private readonly TweetinviService _tweetinviService;

        public TwitterService(TweetinviService tweetinviService)
        {
            _tweetinviService = tweetinviService;
        }

        public async Task<List<UserToCampaignBalanceDTO>> CountTweetsForUsers(CampaignInfo campaign, SocialActivistDTO[]? socialActivists)
        {
            // 1. get all tweets by hashtag
            SearchTweetsV2Response searchResponse = await _tweetinviService.userClient.SearchV2.SearchTweetsAsync(campaign.hashtag);

            // 2. filter posts made by SocialActivists
            List<string>? userTwitterHandles = socialActivists.Select(sa => sa.TwitterHandle).ToList();

            var authors = searchResponse.Includes.Users
                .Where(author => userTwitterHandles.Contains($"@{author.Username}")).ToList();

            var userToCampaign = new List<UserToCampaignBalanceDTO>();

            authors.ForEach(author =>
            {
                int tweetCount = 0;
                // check 1. author 2. that landingPage is included in tweet message
                var tweetsBySa = searchResponse.Tweets
                    .Where(tweet => tweet.AuthorId == author.Id)
                    .Where(tweet => tweet.Text.IndexOf(campaign.landingPage) > -1)
                    .ToList();

                //counting tweets and retweets
                tweetsBySa
                    .ForEach(tweet => tweetCount += 1 + tweet.PublicMetrics.QuoteCount + tweet.PublicMetrics.RetweetCount);

                Console.WriteLine($"Campaign: {campaign.hashtag}\nUser: {author.Username}\nValidated tweets + retweets: {tweetCount}");

                userToCampaign.Add(new UserToCampaignBalanceDTO()
                {
                    Id = socialActivists.Where(sa => sa.TwitterHandle == $"@{author.Username}").Select(sa => sa.UserId).First(),
                    TwitterHandle = socialActivists.Where(sa => sa.TwitterHandle == $"@{author.Username}").Select(sa => sa.TwitterHandle).First(),
                    CampaignHashtag = campaign.hashtag,
                    Balance = tweetCount,
                    UpdateDate = DateTime.Now
                });
            });

            return userToCampaign;

        }
    }
}
