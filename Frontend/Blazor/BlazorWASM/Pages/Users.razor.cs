using BlazorWASM.Entities;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorWASM.Pages
{
    public partial class Users
    {
        [Inject]
        private HttpClient http { get; set; } = default!;

        private BusinessOwner[]? boUsers;
        private NonProfitUser[]? npUsers;
        private SocialActivist[]? saUsers;

        public bool ShowError { get; set; }
        public string Error { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShowError = false;
                //var response = await http.GetAsync("http://localhost:6060/api/campaign/");
                //response.EnsureSuccessStatusCode();

                //users = await response.Content.ReadFromJsonAsync<Campaign[]>();

                boUsers = await http.GetFromJsonAsync<BusinessOwner[]>("/sample-data/BusinessOwners.json");
                npUsers = await http.GetFromJsonAsync<NonProfitUser[]>("/sample-data/NonProfitUsers.json");
                saUsers = await http.GetFromJsonAsync<SocialActivist[]>("/sample-data/SocialActivists.json");

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
