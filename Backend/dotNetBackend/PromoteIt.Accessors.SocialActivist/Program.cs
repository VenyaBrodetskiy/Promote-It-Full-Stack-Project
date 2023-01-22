// !important
// using Models from MainService is not good, this is temporary solution
// later need to remove DB context from MainService at all, only accessors should work directly with DB
using dotNetBackend.Common;
using dotNetBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromoteIt.Accessors.SocialActivist.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<MasaProjectDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        var app = builder.Build();

        // for debug only
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // "applicationUrl": "https://localhost:7170;http://localhost:5292",

        app.MapGet("/get-all-social-activists", async (MasaProjectDbContext db, HttpContext httpContext) =>
        {
            app.Logger.LogInformation("{Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

            try
            {
                var result = await db.SocialActivists
                    .Where(sa => sa.StatusId == (int)Statuses.Active)
                    .Select(sa => ToDto(sa))
                    .ToListAsync();

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex.Message);
                return Results.Problem(ex.Message);
            }
        })
        .WithName("Get All Social Activists")
        .WithOpenApi();

        app.MapPut("/change-balance-user-to-campaign", 
            async (UserToCampaignTwitterInfo userToCampaign, 
                MasaProjectDbContext db, HttpContext httpContext ) =>
            {
                app.Logger.LogInformation("{Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

                try
                {
                    var mutatedLine = await db.UserToCampaignBalances
                        .Where(row => row.UserId == userToCampaign.UserId && row.CampaignId == userToCampaign.CampaignId)
                        .FirstAsync();

                    if (userToCampaign.CurrentTweetCount - mutatedLine.PreviousTweetCount > 0)
                    {
                        app.Logger.LogInformation("\nTweets + Retweets increase from previous check, increasing balance.\n" +
                            "Old Balance: {old}\n" +
                            "New Balance: {new}", mutatedLine.Balance, mutatedLine.Balance + userToCampaign.CurrentTweetCount - mutatedLine.PreviousTweetCount);
                        mutatedLine.Balance += userToCampaign.CurrentTweetCount - mutatedLine.PreviousTweetCount;
                    }
                    else
                    {
                        app.Logger.LogInformation("\nTweets + Retweets didn't increase from previous check, keep current balance.\n" +
                            "Old Balance: {old}\n" +
                            "New Balance: {new}", mutatedLine.Balance, mutatedLine.Balance);
                    }
                    mutatedLine.PreviousTweetCount = userToCampaign.CurrentTweetCount;
                    mutatedLine.UpdateDate = DateTime.Now;
                    mutatedLine.UpdateUserId = userToCampaign.UserId;

                    await db.SaveChangesAsync();

                    return Results.Ok("Changed user balance");

                }
                catch (Exception ex)
                {
                    app.Logger.LogError(ex.Message);
                    return Results.Problem(ex.Message);
                }
            })
       .WithName("Change Social activist balance to 1 campaign")
       .WithOpenApi();

        app.Run();
    }

    private static SocialActivistDTO ToDto(SocialActivist socialActivist)
    {
        return new SocialActivistDTO()
        {
            UserId = socialActivist.UserId,
            TwitterHandle = socialActivist.TwitterHandle,
            Email = socialActivist.Email,
            Address = socialActivist.Address,
            PhoneNumber = socialActivist.PhoneNumber
        };
    }
}