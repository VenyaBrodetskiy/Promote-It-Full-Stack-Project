using BlazorWASM.Entities;
using BlazorWASM.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWASM.Pages
{
    public partial class Home
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private UserCredentials _user = new UserCredentials();
        private AuthToken _token = new AuthToken();

        public bool ShowAuthError { get; set; }
        public string Error { get; set; }

        public async Task OnLoginClick()
        {
            try
            {
                var result = await AuthenticationService.LoginUser(_user);
                NavigationManager.NavigateTo("/");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Couldn't log in");
                Error = ex.Message;
                ShowAuthError = true;
            }
        }
    }
}