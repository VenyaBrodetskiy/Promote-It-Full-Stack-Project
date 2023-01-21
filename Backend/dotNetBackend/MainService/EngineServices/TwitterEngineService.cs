using dotNetBackend.Common;
using dotNetBackend.DTO;
using MainService.Controllers;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MainService.EngineServices
{
    public class TwitterEngineService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TwitterEngineService> _logger;

        public TwitterEngineService(HttpClient httpClient, ILogger<TwitterEngineService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<TweetDTO>?> GetAllTweets()
        {
            // send response to engine service

            try
            {
                _logger.LogInformation("Redirecting to twitter engine microservice: {engine}", Endpoints.TwitterEngineGetAllTweets);

                var response = await _httpClient.GetAsync(Endpoints.TwitterEngineGetAllTweets);
                
                _logger.LogInformation("twitterEngine returned response");

                response.EnsureSuccessStatusCode();

                _logger.LogInformation("parse response to List of TweetDTO");

                var tweets = await response.Content.ReadFromJsonAsync<List<TweetDTO>>();

                return tweets;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


    }
}
