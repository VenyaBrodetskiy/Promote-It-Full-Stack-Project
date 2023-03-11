using Microsoft.AspNetCore.Mvc.Testing;
using dotNetBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IntegrationTest
{
    public class CommonContext : IDisposable
    {
        public HttpClient localClient, httpClient;
        public readonly MasaProjectDbContext db;
        public CommonContext() 
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            localClient = webAppFactory.CreateClient();
            httpClient = new HttpClient();

            var optionsBuilder = new DbContextOptionsBuilder<MasaProjectDbContext>();

            var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
            db = new MasaProjectDbContext(optionsBuilder.Options);
        }

        public void Dispose()
        {
            localClient.Dispose();
            httpClient.Dispose();
            db.Dispose();
        }
    }
}
