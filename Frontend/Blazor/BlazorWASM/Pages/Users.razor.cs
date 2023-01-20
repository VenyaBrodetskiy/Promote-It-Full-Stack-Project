using BlazorWASM.Constants;
using BlazorWASM.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace BlazorWASM.Pages
{
    public partial class Users
    {
        [Inject]
        private HttpClient http { get; set; } = default!;

        [Inject]
        private ILogger<Users> logger { get; set; }

        private BusinessOwner[]? boUsers;
        private NonProfitUser[]? npUsers;
        private SocialActivist[]? saUsers;

        public bool ShowError { get; set; }
        public string Error { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                logger.LogInformation("Component Initialized");
                ShowError = false;

                logger.LogInformation("Sending request to {endpoint}", Endpoints.BusinessOwners);
                var responseBO = await http.GetAsync(Endpoints.BusinessOwners);
                responseBO.EnsureSuccessStatusCode();
                boUsers = await responseBO.Content.ReadFromJsonAsync<BusinessOwner[]>();

                logger.LogInformation("Sending request to {endpoint}", Endpoints.NonProfit);
                var responseNP = await http.GetAsync(Endpoints.NonProfit);
                responseNP.EnsureSuccessStatusCode();
                npUsers = await responseNP.Content.ReadFromJsonAsync<NonProfitUser[]>();

                logger.LogInformation("Sending request to {endpoint}", Endpoints.SocialActivists);
                var responseSA = await http.GetAsync(Endpoints.SocialActivists);
                responseSA.EnsureSuccessStatusCode();
                saUsers = await responseSA.Content.ReadFromJsonAsync<SocialActivist[]>();

                // testing sample data
                // boUsers = await http.GetFromJsonAsync<BusinessOwners[]>("/sample-data/BusinessOwners.json");
                // npUsers = await http.GetFromJsonAsync<NonProfitUser[]>("/sample-data/NonProfitUsers.json");
                // saUsers = await http.GetFromJsonAsync<SocialActivist[]>("/sample-data/SocialActivists.json");

            }
            catch (Exception ex)
            {
                Error = ex.Message;
                ShowError = true;
                Console.WriteLine(ex.Message);
            }
        }

    }
}
