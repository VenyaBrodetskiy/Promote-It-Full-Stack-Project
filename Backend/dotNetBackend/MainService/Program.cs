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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c => {
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
builder.Services.AddAuthorization(options => 
{
    options.AddPolicy(Policies.BusinessOwner, policy => {
        policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.BusinessOwner.ToString("D"));
    });
    options.AddPolicy(Policies.SocialActivist, policy => {
        policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.SocialActivist.ToString("D"));
    });
    options.AddPolicy(Policies.NonprofitOrganization, policy => {
        policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.NonprofitOrganization.ToString("D"));
    });
    options.AddPolicy(Policies.ProlobbyOwner, policy => {
        policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.ProlobbyOwner.ToString("D"));
    });
    options.AddPolicy(Policies.SystemBackendOnly, policy => {
        policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.System.ToString("D"));
    });
    options.AddPolicy(Policies.ProlobbyAndSystem, policy => {
        policy.RequireClaim(TokenClaims.UserTypeId, UserTypes.ProlobbyOwner.ToString("D"), UserTypes.System.ToString("D"));
    });

});
builder.Services.AddCors();

// TODO: add auto IOC here
builder.Services.AddScoped<SocialActivistService>();
builder.Services.AddScoped<UserToCampaignBalanceService>();
builder.Services.AddScoped<ProductToCampaignQtyService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<TransactionValidationService>();
builder.Services.AddScoped<BusinessOwnerService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<DonationValidationService>();


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

