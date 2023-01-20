using Blazored.LocalStorage;
using BlazorWASM.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BlazorWASM.AuthProviders
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;
        private readonly ILogger<AuthStateProvider> _logger;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage, ILogger<AuthStateProvider> logger)
        {
            _http = httpClient;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            _logger = logger;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //await Task.Delay(2000);

            _logger.LogInformation("Trying to read token from local storage");
            var token = await _localStorage.GetItemAsync<string>("token");
            if (token == null) 
            {
                _logger.LogInformation("No token in Local storage, return _anonymous user");
                return _anonymous;
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var claimsIdentity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType");

            // if userTypeId is ProLobbyOwner, add Role: system and give access
            if (claimsIdentity.Claims
                .Where(c => c.Type == TokenClaims.UserTypeId && c.Value == UserTypes.ProlobbyOwner.ToString("D"))
                .Count() == 0)
            {
                _logger.LogInformation("Token doens't have Claim of ProLobbyOwner, return _anonymous user");
                return _anonymous;
            }
            else
            {
                _logger.LogInformation("Token has required Claim, assigning Role: {role}", Roles.ProlobbyOwner);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, Roles.ProlobbyOwner));
            }
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return new AuthenticationState(claimsPrincipal);
        }

        public void NotifyUserAuthentication()
        {
            //var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, login) }, "jwtAuthType"));
            //var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            var authState = GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
