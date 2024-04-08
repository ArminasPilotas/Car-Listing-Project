using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMauiApp.ViewModels
{
    public partial class LogoutViewModel : BaseViewModel
    {
        public LogoutViewModel()
        {
            Logout();
        }

        [RelayCommand]
        async void Logout()
        {
            SecureStorage.Remove("Token");
            App.UserInfo = null;
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }
    }
}
