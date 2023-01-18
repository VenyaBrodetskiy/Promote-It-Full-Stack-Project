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
                //var response = await http.GetAsync("");
                //response.EnsureSuccessStatusCode();

                //tweets = await response.Content.ReadFromJsonAsync<Tweet[]>();

                tweets = await http.GetFromJsonAsync<Tweet[]>("/sample-data/Tweets.json");

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
