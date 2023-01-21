using Microsoft.AspNetCore.Mvc;
using PromoteIt.Accessors.Twitter;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<TweetinviService>();

builder.Services.AddCors();

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
            app.Logger.LogInformation("Sending tweet using Tweetinvi");
            var responseTweet = await tweetinviService.userClient.Tweets.PublishTweetAsync(tweet);
            app.Logger.LogInformation("tweet is published: {tweet}", responseTweet.Text);
            return Results.Ok(responseTweet.Text);
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex.Message);
            return Results.Problem(ex.Message);
        }
    })
.WithName("Create Tweet when Social Activist bought product from Business owner")
.WithOpenApi();

app.MapGet("/getTweetsByHashtag/{hashtag:regex(#[a-zA-Z0-9_])}", 
    async (string hashtag, [FromServices] TweetinviService tweetinviService) =>
    {
        try
        {
            app.Logger.LogInformation("Asking twitter to give all tweets by hashtag {hash}", hashtag);
            SearchTweetsV2Response searchResponse = await tweetinviService.userClient.SearchV2.SearchTweetsAsync(hashtag);
            if (searchResponse.Tweets.Length == 0)
            {
                return Results.Ok();
            }
            else
            {
                return Results.Ok(searchResponse);
            }
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex.Message);
            return Results.Problem(ex.Message);
        }
    })
.WithName("Returns all tweets by hashtag")
.WithOpenApi();


app.Run();
