using Microsoft.AspNetCore.Mvc;
using Tweetinvi.Parameters;
using TwitterAccessor;
using TwitterAccessor.Common;
using TwitterAccessor.Models;
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

        // Login as SystemUser
        app.UseMiddleware<LoginAsSystemUserMiddleware>();

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

        //app.MapPut("/UpdateBalancesByCampaignHashtag", 
        //    async (
        //        [FromBody] CampaignInfo campaign, 
        //        [FromServices] TwitterService twitterService) =>
        //    {
        //        try
        //        {
        //            await twitterService.ChangeUserBalancesForCampaign(campaign);

        //            return Results.Ok();
        //        }
        //        catch (Exception ex)
        //        {
        //            return Results.Problem(ex.Message);
        //        }
        //    })
        //.WithName("Count tweets by campaign")
        //.WithOpenApi();

        app.MapPut("/UpdateBalancesAllCampaigns",
            async ([FromServices] TwitterService twitterService, HttpContext httpContext) =>
            {
                try
                {
                    string? token = httpContext.Request.Headers.Authorization;
                    if (token == null)
                    {
                        return Results.BadRequest("Couldn't find token");
                    }

                    await twitterService.ChangeUserBalancesForAllCampaigns(token);

                    return Results.Ok("All Campaignes checked, all user balances updated");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
        .WithName("Count tweets for all campaigns")
        .WithOpenApi();

        app.MapPut("/StartTwitterChecking/{seconds}",
            (int seconds,
            [FromServices] TwitterService twitterService,
            HttpContext httpContext) =>
            {
                try
                {
                    string? token = httpContext.Request.Headers.Authorization;
                    if (token == null)
                    {
                        return Results.BadRequest("Couldn't find token");
                    }

                    twitterService.StartPeriodicalCheck(seconds, token);

                    return Results.Ok("Started periodical check of twitter");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
        .WithName("Start to check twitter each {seconds}")
        .WithOpenApi();

        app.MapPost("/StopTwitterChecking",
            ([FromServices] TwitterService twitterService) =>
            {
                try
                {
                    twitterService.StopPeriodicalCheck();

                    return Results.Ok("Stopped periodical check of twitter");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
        .WithName("Stopped automatic twitter check")
        .WithOpenApi();


        app.Run();
    }
}

  