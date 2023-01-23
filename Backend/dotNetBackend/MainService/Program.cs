using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using dotNetBackend.AccessorsServices;
using dotNetBackend.EngineServices;
using dotNetBackend.Models;
using dotNetBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using dotNetBackend.Common;
using MainService.EngineServices;
using MainService;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        
        builder.Services.AddConfiguredSwagger();

        builder.Services.AddDbContext<MasaProjectDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

        });

        builder.Services.AddHttpClient();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                string TOKEN_SECRET = builder.Configuration.GetValue<string>("JwtSigningSecret")!;
                var signingKey = Encoding.UTF8.GetBytes(TOKEN_SECRET);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                };
            });

        builder.Services.AddConfiguredAuthorization();
        
        builder.Services.AddCors();

        builder.Services.AddConfiguredServices();
        

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors(
            options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        // added logging of http requests
        app.UseHttpLogging();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
