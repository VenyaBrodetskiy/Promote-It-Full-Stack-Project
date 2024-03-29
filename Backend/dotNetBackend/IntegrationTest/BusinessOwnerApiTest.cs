﻿using ApprovalTests;
using ApprovalTests.Core;
using ApprovalTests.Reporters;
using dotNetBackend.DTO;
using dotNetBackend.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTest
{
    [UseReporter(typeof(VisualStudioReporter))]
    public class BusinessOwnerApiTest : IClassFixture<CommonContext>
    {
        private readonly CommonContext context;
        private readonly string tokenFor10Years = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjQsInVzZXJUeXBlSWQiOjEsImlhdCI6MTY3ODU3OTc5MCwiZXhwIjoxOTk0MTU1NzkwfQ.gwcS99aIQkRFJ0PeYB7cWjXzX5-czfvu3in84zf15-c";
        //private int UserId = 4;
        private readonly string baseUrl = "/api/BusinessOwner";

        public BusinessOwnerApiTest(CommonContext context)
        {
            this.context = context;
            context.localClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenFor10Years);
        }

        [Theory]
        [InlineData("/GetAll")]
        [InlineData("/Get")]
        [InlineData("/GetProducts")]
        public async Task Get_EndpointsReturnSuccessAndCorrectStatusCode(string url)
        {
            // Arrange

            // Act
            var response = await context.localClient.GetAsync($"{baseUrl}{url}");

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var text = await response.Content.ReadAsStringAsync();

            // !! Approval tests - do not work in guthub actions
            //Approvals.RegisterDefaultNamerCreation(() =>
            //    new Test(url.Trim('/'))
            //);
            //Approvals.Verify(text);

        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        public async Task ChangeTransactionState(int id, int stateId)
        {
            // Arrange
            var changeTransaction = new TransactionChangeState()
            {
                Id = id,
                StateId = stateId,
            };

            // Act
            var response = await context.localClient.PutAsJsonAsync($"{baseUrl}/ChangeTransactionState", changeTransaction);
            response.EnsureSuccessStatusCode();

            var transactionState = await context.db.Transactions
                .Where(t => t.Id == id)
                .Select(t => t.StateId)
                .FirstAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(stateId, transactionState);
        }
    }

    public class Test : IApprovalNamer
    {
        private readonly string _name;

        public Test(string name)
        {
            _name = name;
        }
        public string SourcePath => "..\\..\\..\\mytestsfolder";

        public string Name => _name;
    }
}
