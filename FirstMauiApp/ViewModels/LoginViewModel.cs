using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FirstMauiApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
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
                // Call API to attemt a login
                var loginSuccessful = true;

                if (loginSuccessful)
                {

                }

                await DisplayLoginError();
            }

        }

        async Task DisplayLoginError()
        {
            await Shell.Current.DisplayAlert("Invalid Attempt", "Invalid Username or Password", "OK");
            Password = string.Empty;
        }
    }
}
