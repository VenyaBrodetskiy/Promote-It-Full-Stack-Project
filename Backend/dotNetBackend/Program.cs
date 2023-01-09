using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using dotNetBackend.AccessorsServices;
using dotNetBackend.EngineServices;
using dotNetBackend.Models;
using dotNetBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MasaProjectDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    
});
builder.Services.AddHttpClient();

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

// added logging of http requests
app.UseHttpLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();

