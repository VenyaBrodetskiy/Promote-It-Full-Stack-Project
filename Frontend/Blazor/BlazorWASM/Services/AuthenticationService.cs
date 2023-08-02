using Blazored.LocalStorage;
using BlazorWASM.AuthProviders;
using BlazorWASM.Constants;
using BlazorWASM.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorWASM.Services
{
    public interface IAuthenticationService
    {
        public Task<AuthToken> LoginUser(UserCredentials user);
        public Task Logout();
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _http;
        private readonly AuthStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly ILogger _logger;

        public AuthenticationService(
            HttpClient http,
            AuthenticationStateProvider authStateProvider, 
            ILocalStorageService localStorage,
            ILogger<AuthenticationService> logger)
        {
            _http = http;
            _authStateProvider = (AuthStateProvider)authStateProvider;
            _localStorage = localStorage;
            _logger = logger;
        }
        public async Task<AuthToken> LoginUser(UserCredentials user)
        {
            try
            {
                _logger.LogInformation("Sending request to {endpoint}", Endpoints.Login);
                var response = await _http.PostAsJsonAsync(Endpoints.Login, user);
                response.EnsureSuccessStatusCode();
                _logger.LogInformation("Received Success response from server");

                _logger.LogInformation("Reading token from response");
                var token = await response.Content.ReadFromJsonAsync<AuthToken>();
                
                _logger.LogInformation("Saving token to Local Storage");
                await _localStorage.SetItemAsync("token", token.Token);
                
                _logger.LogInformation("Notify User Authentication");
                _authStateProvider.NotifyUserAuthentication();
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: {message}", ex.Message);
                throw;
            }
        }

        public async Task Logout()
        {
            _logger.LogInformation("Deleting token from LocalStorage");
            await _localStorage.RemoveItemAsync("token");
            _authStateProvider.NotifyUserLogout();
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
