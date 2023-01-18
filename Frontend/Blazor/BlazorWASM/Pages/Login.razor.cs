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
                var result = await AuthenticationService.LoginUser(_user);

                var authState = await AuthState;
                var role = authState.User.FindFirst(ClaimTypes.Role);
                if (role != null && role.Value == "ProLobbyOwner")
                {
                    NavigationManager.NavigateTo("/home");
                }
                else
                {
                    Error = "It seems that you are not a ProLobby owner. Please change account";
                    ShowAuthError = true;
                    StateHasChanged();

                }
                loading = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Couldn't log in");
                Error = ex.Message;
                ShowAuthError = true;
                loading = false;
            }
        }
    }
}