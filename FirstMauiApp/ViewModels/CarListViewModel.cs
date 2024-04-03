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
            GetCarListAsync().Wait();
        }

        [ObservableProperty]
        bool isRefreshing;
        [ObservableProperty]
        string make;
        [ObservableProperty]
        string model;
        [ObservableProperty]
        string vin;

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
        async Task GetCarDetails(int id)
        {
            if (id == default) return;

            await Shell.Current.GoToAsync($"{nameof(CarDetailsPage)}?Id={id}", true);
        }

        [RelayCommand]
        async Task AddCarAsync()
        {
            if (string.IsNullOrEmpty(Make) ||  string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(Vin))
            {
                await Shell.Current.DisplayAlert("Invalid Data", "Please insert valid data", "Ok");
                return;
            }

            var car = new Car
            {
                Make = Make,
                Model = Model,
                Vin = Vin
            };

            App.CarService.AddCar(car);
            await Shell.Current.DisplayAlert("Success", App.CarService.StatusMessage, "Ok");
            await GetCarListAsync();
        }

        [RelayCommand]
        async Task DeleteCarAsync(int id)
        {
            if (id == 0)
            {
                await Shell.Current.DisplayAlert("Invalid Record", "Please try again", "Ok");
                return;
            }

            var result = App.CarService.DeleteCar(id);
            if (result == default) await Shell.Current.DisplayAlert("Failed", "Please insert valid data", "Ok");
            else
            {
                await Shell.Current.DisplayAlert("Deletion Successful", "Record Removed Successfully", "Ok");
                await GetCarListAsync();
            }
        }

        [RelayCommand]
        async Task UpdateCarAsync(int id)
        {
            return;
        }
    }
}
