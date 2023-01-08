using Microsoft.AspNetCore.Mvc;
using Tweetinvi.Parameters;
using TwitterAccessor;
using TwitterAccessor.Common;
using TwitterAccessor.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<TweetinviService>();
        builder.Services.AddSingleton<TwitterService>();
        builder.Services.AddHttpClient();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapPost("/create-tweet/{socialActivistTwitterHandle:regex(@[a-zA-Z0-9_]{{0,15}})}/{businessOwnerTwitterHandle:regex(@[a-zA-Z0-9_]{{0,15}})}", 
            async (
                string socialActivistTwitterHandle, 
                string businessOwnerTwitterHandle, 
                [FromServices] TweetinviService tweetinviService) =>
            {
                var tweet = new PublishTweetParameters()
                {
                    Text = $"This tweet is created automatically. {socialActivistTwitterHandle} used points to buy {businessOwnerTwitterHandle} product"
                };

                try
                {
                    var responseTweet = await tweetinviService.userClient.Tweets.PublishTweetAsync(tweet);
                    app.Logger.LogInformation("tweet is published: {tweet}", responseTweet.Text);
                    return Results.Ok(responseTweet.Text);
                }
                catch (Exception ex)
                {
                    app.Logger.LogInformation(ex.Message);
                    return Results.Problem(ex.Message);
                }
            })
        .WithName("Create Tweet when Social Activist bought product from Business owner")
        .WithOpenApi();

        app.MapPost("/GetTweetsCountForCampaignHashtag", 
            async (
                [FromBody] CampaignInfo campaign, 
                [FromServices] HttpClient httpClient, 
                [FromServices] TwitterService twitterService) =>
            {
                // 0. get all users (social activists)
                // 1. get all tweets by hashtag
                // 2. sort those which made by users of my platform
                // 3. check if they are made by rules: contains link + hashtag
                // 4. count tweets and retweets for each user
                // 5. if retweets increase from last time, increase User balance

                // 0. get all users (social activists)
                var socialActivistsResponse = await httpClient.GetAsync(Const.APIGetSocialActivists);
                SocialActivistDTO[]? socialActivists = await socialActivistsResponse.Content.ReadFromJsonAsync<SocialActivistDTO[]>();

                var userToCampaign = await twitterService.CountTweetsForUsers(campaign,  socialActivists);

                return Results.Ok(userToCampaign);
                // add or change balance in UserToCampaignBalance
            })
        .WithName("Count tweets by campaign")
        .WithOpenApi();


        app.Run();
    }
}