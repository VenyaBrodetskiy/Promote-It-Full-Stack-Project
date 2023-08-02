using BlazorWASM.Constants;
using BlazorWASM.Entities;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorWASM.Pages
{
    public partial class Tweets
    {
        [Inject]
        private HttpClient http { get; set; } = default!;
        [Inject]
        private ILogger<Tweets> logger { get; set; }
        
        private Tweet[]? tweets;

        public bool ShowError { get; set; }
        public string Error { get; set; } = "";
        public bool ShowSuccess { get; set; }
        public string? Success { get; set; }

        public int defaultInterval = 300; // 5 minutes

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShowError = false;

                // for tests
                // tweets = await http.GetFromJsonAsync<Tweet[]>("/sample-data/Tweets.json");
                logger.LogInformation("Component initialized. Sending request to {endpoint}", Endpoints.AllTweets);

                var response = await http.GetAsync(Endpoints.AllTweets);
                response.EnsureSuccessStatusCode();

                tweets = await response.Content.ReadFromJsonAsync<Tweet[]>();


            }
            catch (Exception ex)
            {
                Error = ex.Message;
                ShowError = true;
                logger.LogError(ex.Message);
            }
        }

        private async Task OnLaunch()
        {
            try
            {
                ShowSuccess = false;

                logger.LogInformation("Sending request to {endpoint}", Endpoints.StartTwitterChecking);

                var response = await http.PutAsync($"{Endpoints.StartTwitterChecking}/{defaultInterval}", null);
                response.EnsureSuccessStatusCode();
                
                Success = await response.Content.ReadAsStringAsync();
                ShowSuccess = true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                ShowError = true;
                logger.LogError(ex.Message);
            }
        }

        private async Task OnStop()
        {
            try
            {
                ShowSuccess = false;

                logger.LogInformation("Sending request to {endpoint}", Endpoints.StopTwitterChecking);

                var response = await http.PostAsync(Endpoints.StopTwitterChecking, null);
                response.EnsureSuccessStatusCode();

                Success = await response.Content.ReadAsStringAsync();
                ShowSuccess = true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                ShowError = true;
                logger.LogError(ex.Message);
            }
        }

        private void OnCloseSuccess()
        {
            ShowSuccess = false;
        }
    }
}
