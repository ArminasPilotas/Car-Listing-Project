using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstMauiApp.Models;
using FirstMauiApp.Services;
using FirstMauiApp.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FirstMauiApp.ViewModels
{
    public partial class CarListViewModel : BaseViewModel
    {
        public ObservableCollection<Car> Cars { get; private set; } = [];

        public CarListViewModel()
        {
            Title = "Car List";
        }

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task GetCarListAsync()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                if (Cars.Any()) Cars.Clear();

                var cars = App.CarService.GetCars();
                foreach (var car in cars) Cars.Add(car);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get cars: {ex.Message}.");
                await Shell.Current.DisplayAlert("Error", "Failed to retrieve list of cars.", "Ok");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        async Task GetCarDetails(Car car)
        {
            if (car is null) return;

            await Shell.Current.GoToAsync(nameof(CarDetailsPage), true, new Dictionary<string, object>
            {
                {nameof(Car), car }
            });
        }
    }
}
