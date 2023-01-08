using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Timers;
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

                try
                {
                    // get all users (social activists)
                    var socialActivistsResponse = await httpClient.GetAsync(Const.ApiGetSocialActivists);
                    SocialActivistDTO[]? socialActivists = await socialActivistsResponse.Content.ReadFromJsonAsync<SocialActivistDTO[]>();

                    var userToCampaigns = await twitterService.CountTweetsForUsers(campaign, socialActivists);

                    //var responseMessage = await httpClient.PostAsJsonAsync(Const.ApiPostUpdateUserBalance, userToCampaigns[0]);
                    //app.Logger.LogInformation("Status: {status}, Message: {message}", responseMessage.StatusCode, responseMessage.Content.ToString());

                    foreach (var userToCampaign in userToCampaigns) 
                    {
                        var responseMessage = await httpClient.PostAsJsonAsync(Const.ApiPostUpdateUserBalance, userToCampaign);
                        app.Logger.LogInformation("Status: {status}, Message: {message}", responseMessage.StatusCode, responseMessage.Content.ToString());
                    };

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
        .WithName("Count tweets by campaign")
        .WithOpenApi();

        var timer = new System.Timers.Timer(TimeSpan.FromSeconds(10).TotalMilliseconds);
        timer.Enabled = true;
        timer.Elapsed += OnTimerEvent;

        app.Run();
    }

    private static async void OnTimerEvent(Object? source, ElapsedEventArgs e)
    {
        Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);

        var campaign = new CampaignInfo()
        {
            CampaignId = 4,
            Hashtag = "#trytofindthishash01",
            LandingPage = "old"
        };
        var httpClient = new HttpClient();
        
        await httpClient.PostAsJsonAsync("https://localhost:7133/GetTweetsCountForCampaignHashtag", campaign);
    }
}