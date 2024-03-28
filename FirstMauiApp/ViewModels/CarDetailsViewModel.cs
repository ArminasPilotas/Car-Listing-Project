using CommunityToolkit.Mvvm.ComponentModel;
using FirstMauiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMauiApp.ViewModels
{
    [QueryProperty(nameof(Car), "Car")]
    public partial class CarDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        Car car;
    }
}
