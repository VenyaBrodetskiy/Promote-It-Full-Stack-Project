using Microsoft.AspNetCore.Mvc.Testing;
using dotNetBackend.Models;
using Microsoft.EntityFrameworkCore;

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
            optionsBuilder.UseSqlServer("Server=tcp:masaproject.database.windows.net,1433;Initial Catalog=MasaProject;Persist Security Info=False;User ID=MasaProject;Password=MilaVenya2611;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
