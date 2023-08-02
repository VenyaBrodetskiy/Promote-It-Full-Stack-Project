using BlazorWASM.Constants;
using BlazorWASM.Entities;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorWASM.Pages
{
    public partial class Users
    {
        [Inject]
        private HttpClient http { get; set; } = default!;

        [Inject]
        private ILogger<Users> logger { get; set; } = default!;

        private BusinessOwner[]? boUsers;
        private NonProfitUser[]? npUsers;
        private SocialActivist[]? saUsers;

        public int CurrentPageBO { get; set; } = 1;
        public int CurrentPageNP { get; set; } = 1;
        public int CurrentPageSA { get; set; } = 1;
        public int TotalPagesBO { get; set; } = 1;
        public int TotalPagesNP { get; set; } = 1;
        public int TotalPagesSA { get; set; } = 1;
        public enum UserType
        {
            BusinessOwner,
            NonProfit,
            SocialActivist
        }

        public int ItemsPerPage { get; set; } = 5;
        public bool ShowError { get; set; }
        public string Error { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                logger.LogInformation("Component Initialized");
                ShowError = false;

                await FetchData<BusinessOwner>(UserType.BusinessOwner, CurrentPageBO, Endpoints.BusinessOwners);

                await FetchData<NonProfitUser>(UserType.NonProfit, CurrentPageNP, Endpoints.NonProfit);

                await FetchData<SocialActivist>(UserType.SocialActivist, CurrentPageSA, Endpoints.SocialActivists);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                ShowError = true;
                Console.WriteLine(ex.Message);
            }
        }

        public async Task ChangePage(UserType userType, int targetPage)
        {
            switch (userType)
            {
                case UserType.BusinessOwner:
                    CurrentPageBO = targetPage;
                    await FetchData<BusinessOwner>(userType, targetPage, Endpoints.BusinessOwners);
                    break;
                case UserType.NonProfit:
                    CurrentPageNP = targetPage;
                    await FetchData<NonProfitUser>(userType, targetPage, Endpoints.NonProfit);
                    break;
                case UserType.SocialActivist:
                    CurrentPageSA = targetPage;
                    await FetchData<SocialActivist>(userType, targetPage, Endpoints.SocialActivists);
                    break;
            }
        }

        public async Task FetchData<T>(UserType userType, int page, string endpoint) where T : class
        {
            var skip = (page - 1) * ItemsPerPage;

            logger.LogInformation("Sending request to {endpoint}", endpoint);
            var response = await http.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<T[]>();

            var totalPages = (int)Math.Ceiling((double)users!.Length / ItemsPerPage);

            users = users.Skip(skip).Take(ItemsPerPage).ToArray();

            switch (userType)
            {
                case UserType.BusinessOwner:
                    boUsers = users as BusinessOwner[];
                    TotalPagesBO = totalPages;
                    break;
                case UserType.NonProfit:
                    npUsers = users as NonProfitUser[];
                    TotalPagesNP = totalPages;
                    break;
                case UserType.SocialActivist:
                    saUsers = users as SocialActivist[];
                    TotalPagesSA = totalPages;
                    break;
            }
        }
    }
}
