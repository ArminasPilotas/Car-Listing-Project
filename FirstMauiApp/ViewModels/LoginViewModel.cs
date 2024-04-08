using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstMauiApp.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FirstMauiApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(CarApiService carApiService)
        {
            this.carApiService = carApiService;
        }

        private CarApiService carApiService;

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;

        [RelayCommand]
        async Task Login()
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayLoginError();
            }
            else
            {
                var response = await carApiService.LoginAsync(new LoginModel(username, password));
                await Shell.Current.DisplayAlert("Login Attempt Result", carApiService.StatusMessage, "Ok");
                Password = string.Empty;

                if (!string.IsNullOrEmpty(response.Token))
                {
                    await SecureStorage.SetAsync("Token", response.Token);

                    var jsonToken = new JwtSecurityTokenHandler().ReadToken(response.Token) as JwtSecurityToken;

                    App.UserInfo = new Models.UserInfo()
                    {
                        Username = Username,
                        Role = jsonToken.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role))?.Value
                    };

                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
                else
                {
                    await DisplayLoginError();
                }
            }
        }

        async Task DisplayLoginError()
        {
            await Shell.Current.DisplayAlert("Invalid Attempt", "Invalid Username or Password", "OK");
            Password = string.Empty;
        }
    }
}
