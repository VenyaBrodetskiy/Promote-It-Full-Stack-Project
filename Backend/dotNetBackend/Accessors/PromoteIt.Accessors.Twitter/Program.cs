using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromoteIt.Accessors.Twitter;
using PromoteIt.Accessors.Twitter.Models;
using System.Net.Http;
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

// for debug only
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// endpoints: https://localhost:7128 , http://localhost:5233

app.MapPost("/create-tweet/{socialActivistTwitterHandle:regex(@[a-zA-Z0-9_]{{0,15}})}/{businessOwnerTwitterHandle:regex(@[a-zA-Z0-9_]{{0,15}})}",
    async (
        string socialActivistTwitterHandle,
        string businessOwnerTwitterHandle,
        [FromServices] TweetinviService tweetinviService,
        HttpContext httpContext) =>
    {
        app.Logger.LogInformation("{Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

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
    async (string hashtag, [FromServices] TweetinviService tweetinviService, HttpContext httpContext) =>
    {
        app.Logger.LogInformation("{Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

        try
        {
            app.Logger.LogInformation("Asking twitter to give all tweets by hashtag: {hash}", hashtag);
            SearchTweetsV2Response searchResponse = await tweetinviService.userClient.SearchV2.SearchTweetsAsync(hashtag);
            if (searchResponse.Tweets.Length == 0)
            {
                app.Logger.LogInformation("Success, but not tweets found by hashtag: {hash}", hashtag);
                return Results.Ok();
            }
            else
            {
                app.Logger.LogInformation("Success, converting to DTO");

                // convert to DTO
                var result = new TwitterResponseDTO();
                result.Tweets = searchResponse.Tweets.Select(t => new TweetDTO()
                {
                    AuthorId = t.AuthorId,
                    CreatedAt = t.CreatedAt.DateTime,
                    Id = t.Id,
                    Text = t.Text,
                    PublicMetrics = new PublicMetricsDTO()
                    {
                        LikeCount = t.PublicMetrics.LikeCount,
                        QuoteCount = t.PublicMetrics.QuoteCount,
                        ReplyCount = t.PublicMetrics.ReplyCount,
                        RetweetCount = t.PublicMetrics.RetweetCount,
                    },
                    Url = t.Entities.Urls?.First().ExpandedUrl
                }).ToList();

                result.Authors = searchResponse.Includes.Users.Select(u => new AuthorDTO()
                {
                    Id = u.Id,
                    Name = u.Name,
                    UserName = u.Username
                }).ToList();

                return Results.Ok(result);
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
