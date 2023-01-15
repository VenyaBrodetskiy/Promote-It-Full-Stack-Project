using IntegrationTest.TestHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using dotNetBackend.DTO;
using TwitterAccessor.Common;
using TwitterAccessor.Models;
using Newtonsoft.Json.Linq;

namespace IntegrationTest
{
    public class CommonContext : IDisposable
    {
        public HttpClient localClient, httpClient;
        public CommonContext() 
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            localClient = webAppFactory.CreateClient();
            httpClient = new HttpClient();
        }

        public void Dispose()
        {
            localClient.Dispose();
            httpClient.Dispose();
        }
    }

    [TestCaseOrderer("IntegrationTest.TestHelpers.PriorityOrderer", "IntegrationTest")]
    public class SocialActivistApiTest : IClassFixture<CommonContext>
    {
        private readonly CommonContext context;
        private readonly string tokenFor10Years = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjMsInVzZXJUeXBlSWQiOjIsImlhdCI6MTY3MzcyNjg5MCwiZXhwIjoxOTg5MzAyODkwfQ.OkbwcrQBELJuwcBd8-qRGDfel9TQPpin_kSnODVf1aU";

        public SocialActivistApiTest(CommonContext context)
        {
            this.context = context;
            context.localClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenFor10Years);

        }

        // This works only on local machine
        //[Theory, TestPriority(-1)]
        //[InlineData("socialActivist", "socialactivist")]
        //public async Task AuthenticateSA(string login, string password)
        //{
        //    // arrange
        //    var authCredentials = new AuthCredentials()
        //    {
        //        Login = login,
        //        Password = password
        //    };

        //    // act
        //    var response = await context.httpClient.PostAsJsonAsync(Const.ApiLogin, authCredentials);
        //    TokenModel? token = await response.Content.ReadFromJsonAsync<TokenModel>();

        //    // assert
        //    response.EnsureSuccessStatusCode();
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);


        //    context.localClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        //}

        [Fact]
        public async Task Get()
        {
            // arrange

            // act
            var response = await context.localClient.GetAsync($"/api/SocialActivist/Get");
            var body = await response.Content.ReadFromJsonAsync<SocialActivistDTO>();

            // assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<SocialActivistDTO>(body);
        }
    }
}
