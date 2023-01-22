using IntegrationTest.TestHelpers;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using dotNetBackend.DTO;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest
{

    [TestCaseOrderer("IntegrationTest.TestHelpers.PriorityOrderer", "IntegrationTest")]
    public class SocialActivistApiTest : IClassFixture<CommonContext>
    {
        private readonly CommonContext context;
        private readonly string tokenFor10Years = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjMsInVzZXJUeXBlSWQiOjIsImlhdCI6MTY3MzcyNjg5MCwiZXhwIjoxOTg5MzAyODkwfQ.OkbwcrQBELJuwcBd8-qRGDfel9TQPpin_kSnODVf1aU";
        private int UserId = 3;
        private readonly string baseUrl = "/api/SocialActivist";
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
        //    var response = await context.httpClient.PostAsJsonAsync(Endpoints.ApiLogin, authCredentials);
        //    TokenModel? token = await response.Content.ReadFromJsonAsync<TokenModel>();

        //    // assert
        //    response.EnsureSuccessStatusCode();
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);


        //    context.localClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        //}

        
        [Theory]
        [InlineData("/Get")]
        [InlineData("/GetBalance")]
        public async Task Get_EndpointsReturnSuccessAndCorrectStatusCode(string url)
        {
            // Arrange

            // Act
            var response = await context.localClient.GetAsync($"{baseUrl}{url}");

            // Assert
            response.EnsureSuccessStatusCode(); 

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateTransaction()
        {
            // arrange
            var newTransaction = new TransactionRequest()
            {
                ProductId = 1,
                CampaignId = 1,
                StateId = 1,
            };
            int productPrice = await context.db.Products.Where(p => p.Id == newTransaction.ProductId).Select(p => p.Price).FirstAsync();
            int productQtyBefore = await context.db.ProductToCampaignQties
                .Where(p => p.ProductId == newTransaction.ProductId && p.CampaignId == newTransaction.CampaignId)
                .Select(p => p.ProductQty).FirstAsync();
            int userBalanceBefore = await context.db.UserToCampaignBalances
                .Where(u => u.UserId == UserId && u.CampaignId == newTransaction.CampaignId)
                .Select(u => u.Balance).FirstAsync();

            // act
            var response = await context.localClient.PostAsJsonAsync($"{baseUrl}/CreateTransaction", newTransaction);
            response.EnsureSuccessStatusCode();
            TransactionResponse? responseBody = await response.Content.ReadFromJsonAsync<TransactionResponse>();

            var transactionAdded = await context.db.Transactions
                .Where(t => t.Id == responseBody!.TransactionId)
                .Select(t => new TransactionRequest()
                {
                    ProductId = t.ProductId,
                    CampaignId = t.CampaignId,
                    StateId = t.StateId,
                })
                .FirstAsync();

            // assert
            Assert.Equal(newTransaction, transactionAdded);
            Assert.Equal(userBalanceBefore - productPrice, responseBody!.NewBalance);
        }
    }
}
