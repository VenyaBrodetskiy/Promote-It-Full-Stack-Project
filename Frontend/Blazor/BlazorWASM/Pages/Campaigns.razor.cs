using BlazorWASM.Entities;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorWASM.Pages
{
    public partial class Campaigns
    {
        [Inject]
        private HttpClient http { get; set; } = default!;

        private Campaign[]? campaigns;

        public bool ShowError { get; set; }
        public string Error { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShowError = false;
                var response = await http.GetAsync("http://localhost:6060/api/campaign/");
                response.EnsureSuccessStatusCode();

                campaigns = await response.Content.ReadFromJsonAsync<Campaign[]>();

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
