using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using PromoteIt.Engines.Twitter.Models;
using PromoteIt.Engines.Twitter.Common;

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
                var result = await GetAllTweets(app.Logger, httpClient);
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

        app.Run();
    }

    private static async Task<List<FilteredTweetDTO>> GetAllTweets(ILogger logger, HttpClient httpClient)
    {
        try
        {
            // 1. get all campaigns
            logger.LogInformation("Getting all campaigns. Sending request to {accessor}", Endpoints.AccessorGetCampaigns);
            var campaignsResponse = await httpClient.GetAsync(Endpoints.AccessorGetCampaigns);
            campaignsResponse.EnsureSuccessStatusCode();

            List<CampaignInfo>? campaigns = await campaignsResponse.Content.ReadFromJsonAsync<List<CampaignInfo>>();
            if (campaigns == null || campaigns.Count == 0)
            {
                logger.LogError($"Couldn't read campaigns from response body");
                throw new Exception($"Couldn't read campaigns from response body");
            }

            // 2. get all users (social activists)
            logger.LogInformation("Getting all social activists. Sending request to {accessor}", Endpoints.AccessorGetSocialActivists);
            var socialActivistsResponse = await httpClient.GetAsync(Endpoints.AccessorGetSocialActivists);
            socialActivistsResponse.EnsureSuccessStatusCode();

            List<SocialActivistDTO>? socialActivists = await socialActivistsResponse.Content.ReadFromJsonAsync<List<SocialActivistDTO>>();
            if (socialActivists == null)
            {
                logger.LogError($"Couldn't read social activists from response body");
                throw new Exception($"Couldn't read social activists from response body");
            }

            // 3. for each campaign check tweets by campaign hashtag
            logger.LogInformation("Start iterating by campaign to get tweets by hashtag");
            var AllTweets = new List<FilteredTweetDTO>();
            foreach (var campaign in campaigns)
            {
                logger.LogInformation("Getting tweets by hashcode. Sending request to {accessor} with param: {param}", Endpoints.AccessorGetTweetsByHashtag, campaign.Hashtag);

                var response = await httpClient.GetAsync($"{Endpoints.AccessorGetTweetsByHashtag}/%23{campaign.Hashtag.TrimStart('#')}");
                response.EnsureSuccessStatusCode();

                try
                {
                    TwitterResponseDTO? twitterResponse = await response.Content.ReadFromJsonAsync<TwitterResponseDTO>();

                    logger.LogInformation("Filtering and parsing twitter response");
                    List<FilteredTweetDTO> filteredTweetsByHashtag = FilterTwitterResponse(twitterResponse!, campaign, socialActivists);

                    AllTweets.AddRange(filteredTweetsByHashtag);
                }
                catch (Exception ex)
                {
                    logger.LogInformation("Probably couldn't find any tweet with this hashtag", ex.Message);
                }
            }
            return AllTweets;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static List<FilteredTweetDTO> FilterTwitterResponse(
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
                    Body = tweet.Text
                }).ToList();
            result.AddRange(tweetsBySa);
        });

        return result;
    }
}