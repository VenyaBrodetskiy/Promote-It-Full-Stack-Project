using BlazorWASM.Entities;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorWASM.Pages
{
    public partial class Tweets
    {
        [Inject]
        private HttpClient http { get; set; } = default!;

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

                var response = await http.GetAsync("https://localhost:7133/getAllTweets");
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
