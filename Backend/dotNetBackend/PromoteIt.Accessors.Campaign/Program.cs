// !important
// using Models from MainService is not good, this is temporary solution
// later need to remove DB context from MainService at all, only accessors should work directly with DB
using dotNetBackend.Common;
using dotNetBackend.Models;
using Microsoft.EntityFrameworkCore;
using PromoteIt.Accessors.Campaign.Models;

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

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // "applicationUrl": "https://localhost:7009;http://localhost:5225",

        app.MapGet("/get-all-campaigns", async (MasaProjectDbContext db, HttpContext httpContext) =>
        {
            app.Logger.LogInformation("{Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

            try
            {
                var result = await db.Campaigns
                    .Where(c => c.StatusId == (int)Statuses.Active)
                    .Select(c => ToDto(c))
                    .ToListAsync();

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex.Message);

                return Results.Problem(ex.Message);
            }
        })
        .WithName("Get all campaigns")
        .WithOpenApi();

        app.Run();
    }

    private static CampaignDTO ToDto(Campaign campaign)
    {
        return new CampaignDTO()
        {
            Id = campaign.Id,
            Hashtag = campaign.Hashtag,
            LandingPage = campaign.LandingPage,
            NonProfitOrganizationName = campaign.Hashtag
        };
    }
}