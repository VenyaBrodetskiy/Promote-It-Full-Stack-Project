using BlazorWASM.Constants;
using BlazorWASM.Entities;
using BlazorWASM.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorWASM.Pages
{
    public partial class Login
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILogger<Login> Logger { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }

        private UserCredentials _user = new UserCredentials();
        private AuthToken _token = new AuthToken();

        public bool ShowAuthError { get; set; }
        private bool loading { get; set; }

        public string Error { get; set; } = "";

        
        public async Task OnLoginClick()
        {
            try
            {
                loading = true;
                ShowAuthError = false;
                Logger.LogInformation("Login user started...");

                var result = await AuthenticationService.LoginUser(_user);

                Logger.LogInformation("Asking for AuthState");

                var authState = await AuthState;
                var role = authState.User.FindFirst(ClaimTypes.Role);
                if (role != null && role.Value == Roles.ProlobbyOwner)
                {
                    Logger.LogInformation("Role is {Role}. Navigating to {Page}", Roles.ProlobbyOwner, Page.Home);
                    NavigationManager.NavigateTo(Page.Home);
                }
                else
                {
                    Error = "It seems that you are not a ProLobby owner. Please change account";
                    ShowAuthError = true;
                    Logger.LogWarning(Error);

                    StateHasChanged();
                }
                loading = false;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                ShowAuthError = true;
                loading = false;
                Logger.LogError("Couldn't log in, Exception message: {message}", Error);
            }
        }
    }
}