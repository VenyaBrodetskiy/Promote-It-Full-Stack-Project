// !important
// using Models from MainService is not good, this is temporary solution
// later need to remove DB context from MainService at all, only accessors should work directly with DB
using dotNetBackend.Common;
using dotNetBackend.Models;
using Microsoft.AspNetCore.Http;
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

        // Configure the HTTP request pipeline.
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