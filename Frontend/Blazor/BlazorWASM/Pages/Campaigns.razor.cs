using BlazorWASM.Constants;
using BlazorWASM.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace BlazorWASM.Pages
{
    public partial class Campaigns
    {
        [Inject]
        private HttpClient http { get; set; } = default!;

        [Inject]
        private ILogger<Campaigns> logger { get; set; }

        private Campaign[]? campaigns;

        public bool ShowError { get; set; }
        public string Error { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShowError = false;
                logger.LogInformation("Component initialized. Sending request to {endpoint}", Endpoints.Campaigns);
                var response = await http.GetAsync(Endpoints.Campaigns);
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
