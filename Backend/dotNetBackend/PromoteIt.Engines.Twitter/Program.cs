using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using PromoteIt.Engines.Twitter.Models;
using PromoteIt.Engines.Twitter.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // "applicationUrl": "https://localhost:7109;http://localhost:5274",

        app.MapGet("/get-all-tweets", async (HttpClient httpClient, HttpContext httpContext) =>
        {
            app.Logger.LogInformation("{Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

            try
            {
                // 1. get all campaigns
                var campaigns = await Helpers.GetAllCampaigns(app.Logger, httpClient);

                // 2. get all users (social activists)
                var socialActivists = await Helpers.GetAllSocialActivists(app.Logger, httpClient);

                // 3. get all tweets, filtered by social activists
                var result = await GetAndParseAllTweets<FilteredTweetDTO>(
                    TwitterResponseToFilteredTweets, campaigns, socialActivists,
                    app.Logger, httpClient);

                if (result == null)
                {
                    return Results.Ok();
                }
                else
                {
                    return Results.Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithName("GetAllTweets")
        .WithOpenApi();

        app.MapPut("/update-all-balances", async (HttpClient httpClient, HttpContext httpContext) =>
        {
            app.Logger.LogInformation("{Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

            try
            {
                // 1. get all campaigns
                var campaigns = await Helpers.GetAllCampaigns(app.Logger, httpClient);

                // 2. get all users (social activists)
                var socialActivists = await Helpers.GetAllSocialActivists(app.Logger, httpClient);

                // 3. get all tweets, parse to userToCampaigns
                var userToCampaigns = await GetAndParseAllTweets<UserToCampaignTwitterInfo>(
                    TwitterResponseToUserToCampaignInfo, campaigns, socialActivists,
                    app.Logger, httpClient);

                // 4. for each line update Balance
                foreach (var userToCampaign in userToCampaigns)
                {
                    app.Logger.LogInformation("\nAsking accessor to change user balance\n" +
                        "UserId: {userId}\n" +
                        "CampaignId: {campaignId}", userToCampaign.UserId, userToCampaign.CampaignId);
                    var response = await httpClient.PutAsJsonAsync(Endpoints.AccessorChangeBalance, userToCampaign);
                    response.EnsureSuccessStatusCode();
                    app.Logger.LogInformation("Changed user balance successfully");
                }

                if (userToCampaigns == null)
                {
                    return Results.Ok();
                }
                else
                {
                    return Results.Ok(userToCampaigns);
                }
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithName("UpdateAllBalances")
        .WithOpenApi();

        app.Run();
    }
    public static async Task<List<T>> GetAndParseAllTweets<T>(
        Func<TwitterResponseDTO, CampaignInfo, List<SocialActivistDTO>, List<T>> FilterTwitterResponse,
        List<CampaignInfo> campaigns,
        List<SocialActivistDTO> socialActivists,
        ILogger logger,
        HttpClient httpClient)
    {
        try
        {
            logger.LogInformation("Start iterating by campaign to get tweets by hashtag");
            var result = new List<T>();
            foreach (var campaign in campaigns)
            {
                logger.LogInformation("Getting tweets by hashcode. Sending request to {accessor} with param: {param}", Endpoints.AccessorGetTweetsByHashtag, campaign.Hashtag);

                var response = await httpClient.GetAsync($"{Endpoints.AccessorGetTweetsByHashtag}/%23{campaign.Hashtag.TrimStart('#')}");
                response.EnsureSuccessStatusCode();

                //response.Content.Headers.ContentLength = 0
                try
                {
                    TwitterResponseDTO? twitterResponse = await response.Content.ReadFromJsonAsync<TwitterResponseDTO>();

                    logger.LogInformation("Filtering and parsing twitter response");
                    List<T> filteredTweetsByHashtag = FilterTwitterResponse(twitterResponse!, campaign, socialActivists);

                    result.AddRange(filteredTweetsByHashtag);
                }
                catch (Exception ex)
                {
                    logger.LogInformation("Probably couldn't find any tweet with this hashtag", ex.Message);
                }
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
    private static List<FilteredTweetDTO> TwitterResponseToFilteredTweets(
        TwitterResponseDTO twitterResponse,
        CampaignInfo campaign,
        List<SocialActivistDTO> socialActivists)
    {
        var result = new List<FilteredTweetDTO>();

        // 1. filter those authors of tweets, who are SocialActivists
        List<string> userTwitterHandles = socialActivists.Select(sa => sa.TwitterHandle).ToList();
        var filteredAuthors = twitterResponse.Authors
            .Where(author => userTwitterHandles.Contains($"@{author.UserName}")).ToList();

        // 2. filter tweets made by SocialActivists
        filteredAuthors.ForEach(author =>
        {
            var tweetsBySa = twitterResponse.Tweets
                .Where(tweet => tweet.AuthorId == author.Id)
                .Select(tweet => new FilteredTweetDTO()
                {
                    User = author.UserName,
                    PublishedOn = tweet.CreatedAt,
                    Link = $"https://twitter.com/{author.UserName}/status/{tweet.Id}",
                    Retweets = tweet.PublicMetrics.RetweetCount + tweet.PublicMetrics.QuoteCount,
                    Body = tweet.Text,
                    Hashtag = campaign.Hashtag,
                }).ToList();
            result.AddRange(tweetsBySa);
        });

        return result;
    }

    private static List<UserToCampaignTwitterInfo> TwitterResponseToUserToCampaignInfo(
        TwitterResponseDTO twitterResponse,
        CampaignInfo campaign,
        List<SocialActivistDTO> socialActivists)
    {
        var result = new List<UserToCampaignTwitterInfo>();

        // 1. filter those authors of tweets, who are SocialActivists
        List<string>? userTwitterHandles = socialActivists.Select(sa => sa.TwitterHandle).ToList();
        var filteredAuthors = twitterResponse.Authors
            .Where(author => userTwitterHandles.Contains($"@{author.UserName}")).ToList();

        filteredAuthors.ForEach(author =>
        {
            int tweetCount = 0;
            // 2. check if tweets are made by rules: contains link + Hashtag
            var tweetsBySa = twitterResponse.Tweets
                .Where(tweet => tweet.AuthorId == author.Id)
                .Where(tweet => tweet.Text.IndexOf(campaign.LandingPage) > -1
                    || tweet.Url == campaign.LandingPage)
                .ToList();

            // 3. count tweets and retweets for each user
            tweetsBySa
                .ForEach(tweet => tweetCount += 1 + tweet.PublicMetrics.QuoteCount + tweet.PublicMetrics.RetweetCount);

            result.Add(new UserToCampaignTwitterInfo()
            {
                UserId = socialActivists.Where(sa => sa.TwitterHandle == $"@{author.UserName}").Select(sa => sa.UserId).First(),
                CampaignId = campaign.Id,
                CurrentTweetCount = tweetCount,
            });
        });

        return result;
    }
}