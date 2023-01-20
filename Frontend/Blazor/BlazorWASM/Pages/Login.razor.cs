using BlazorWASM.Constants;
using BlazorWASM.Entities;
using BlazorWASM.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Linq.Expressions;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorWASM.Pages
{
    public partial class Login
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILogger<Login> logger { get; set; }

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
                logger.LogInformation("Login user started...");

                var result = await AuthenticationService.LoginUser(_user);

                logger.LogInformation("Asking for AuthState");

                var authState = await AuthState;
                var role = authState.User.FindFirst(ClaimTypes.Role);
                if (role != null && role.Value == Roles.ProlobbyOwner)
                {
                    logger.LogInformation("Role is {Role}. Navigating to {Page}", Roles.ProlobbyOwner, Page.Home);
                    NavigationManager.NavigateTo(Page.Home);
                }
                else
                {
                    Error = "It seems that you are not a ProLobby owner. Please change account";
                    ShowAuthError = true;
                    logger.LogWarning(Error);

                    StateHasChanged();
                }
                loading = false;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                ShowAuthError = true;
                loading = false;
                logger.LogError("Couldn't log in, Exception message: {message}", Error);
            }
        }
    }
}