using FirstMauiApp.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FirstMauiApp.ViewModels
{
    public partial class LoadingPageViewModel : BaseViewModel
    {
        public LoadingPageViewModel()
        {
            CheckUserLoginDetails();
        }

        private async void CheckUserLoginDetails()
        {
            var token = await SecureStorage.GetAsync("Token");

            if (string.IsNullOrEmpty(token))
            {
                await GoToLoginPage();
            }
            else
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

                if (jsonToken.ValidTo < DateTime.UtcNow)
                {
                    SecureStorage.Remove("Token");
                    await GoToLoginPage();
                }
                else
                {
                    App.UserInfo = new Models.UserInfo()
                    {
                        Username = jsonToken.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Email))?.Value,
                        Role = jsonToken.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role))?.Value
                    };

                    MenuBuilder.BuildMenu();
                    await GoToMainPage();
                }
            }

        }

        private async Task GoToLoginPage()
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }

        private async Task GoToMainPage()
        {
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
    }
}
