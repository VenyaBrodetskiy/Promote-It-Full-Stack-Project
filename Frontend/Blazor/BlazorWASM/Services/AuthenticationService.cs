using Blazored.LocalStorage;
using BlazorWASM.AuthProviders;
using BlazorWASM.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using static BlazorWASM.Pages.Login;
using static System.Net.WebRequestMethods;

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

        public AuthenticationService(
            HttpClient http,
            AuthenticationStateProvider authStateProvider, 
            ILocalStorageService localStorage)
        {
            _http = http;
            _authStateProvider = (AuthStateProvider)authStateProvider;
            _localStorage = localStorage;
        }
        public async Task<AuthToken> LoginUser(UserCredentials user)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("http://localhost:6060/api/auth/login", user);
                response.EnsureSuccessStatusCode();

                Console.WriteLine(response.StatusCode);
                var token = await response.Content.ReadFromJsonAsync<AuthToken>();

                await _localStorage.SetItemAsync("token", token.Token);
                _authStateProvider.NotifyUserAuthentication();
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("token");
            _authStateProvider.NotifyUserLogout();
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
