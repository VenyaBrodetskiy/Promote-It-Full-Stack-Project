using Blazored.LocalStorage;
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

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _http = httpClient;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //await Task.Delay(2000);

            var token = await _localStorage.GetItemAsync<string>("token");
            if (token == null) 
            {
                return _anonymous;
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var claimsIdentity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType");

            // if userTypeId is ProLobbyOwner, add Role: system and give access
            if (claimsIdentity.Claims.Where(c => c.Type == "userTypeId" && c.Value == "4").Count() == 0)
            {
                return _anonymous;
            }
            else
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "ProLobbyOwner"));
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
