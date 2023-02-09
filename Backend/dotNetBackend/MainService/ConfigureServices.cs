using AutoServiceRegistrator;
using dotNetBackend.AccessorsServices;
using dotNetBackend.Common;
using dotNetBackend.EngineServices;
using dotNetBackend.Services;
using MainService.EngineServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace MainService
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
                }
            });
            });

            return services;
        }

        public static IServiceCollection AddConfiguredAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.BusinessOwner, policy =>
                {
                    policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.BusinessOwner.ToString("D"));
                });
                options.AddPolicy(Policies.SocialActivist, policy =>
                {
                    policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.SocialActivist.ToString("D"));
                });
                options.AddPolicy(Policies.NonprofitOrganization, policy =>
                {
                    policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.NonprofitOrganization.ToString("D"));
                });
                options.AddPolicy(Policies.ProlobbyOwner, policy =>
                {
                    policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.ProlobbyOwner.ToString("D"));
                });
                options.AddPolicy(Policies.SystemBackendOnly, policy =>
                {
                    policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.System.ToString("D"));
                });
                options.AddPolicy(Policies.ProlobbyAndSystem, policy =>
                {
                    policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.ProlobbyOwner.ToString("D"), UserTypes.System.ToString("D"));
                });
            });

            return services;
        }

        public static IServiceCollection AddConfiguredServices(this IServiceCollection services)
        {
            services.RegisterScoped();
            services.RegisterSingletons();

            return services;
        }
    }
}
