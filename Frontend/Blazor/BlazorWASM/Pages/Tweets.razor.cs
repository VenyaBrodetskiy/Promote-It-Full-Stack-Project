using BlazorWASM.Constants;
using BlazorWASM.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
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
                Console.WriteLine(ex.Message);
            }
        }
    }
}
