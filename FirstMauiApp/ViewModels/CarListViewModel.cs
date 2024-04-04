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
        const string editButtonText = "Update Car";
        const string createButtonText = "Add Car";
        private readonly CarApiService carApiService;
        string message = string.Empty;
        public ObservableCollection<Car> Cars { get; private set; } = [];

        public CarListViewModel(CarApiService carApiService)
        {
            this.carApiService = carApiService;
            Title = "Car List";
            AddEditButtonText = createButtonText;
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
        [ObservableProperty]
        string addEditButtonText;
        [ObservableProperty]
        int carId;

        [RelayCommand]
        async Task GetCarListAsync()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                if (Cars.Any()) Cars.Clear();
                var cars = await carApiService.GetCarsAsync();
                foreach (var car in cars) Cars.Add(car);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get cars: {ex.Message}.");
                await ShowAlert("Failed to retrieve list of cars.");
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
        async Task SaveCarAsync()
        {
            if (string.IsNullOrEmpty(Make) ||  string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(Vin))
            {
                await ShowAlert("Please insert valid data");
                return;
            }

            var car = new Car
            {
                Make = Make,
                Model = Model,
                Vin = Vin
            };

            if (CarId != 0)
            {
                await carApiService.UpdateCarAsync(CarId, car);
                message = carApiService.StatusMessage;
            }
            else
            {
                await carApiService.AddCarAsync(car);
                message = carApiService.StatusMessage;
            }
            await ShowAlert(message);
            await GetCarListAsync();
            await ClearFormAsync();
        }

        [RelayCommand]
        async Task DeleteCarAsync(int id)
        {
            if (id == 0)
            {
                await ShowAlert("Please try again");
                return;
            }

            await carApiService.DeleteCarAsync(id);
            message = carApiService.StatusMessage;
            
            await ShowAlert(message);
            await GetCarListAsync();
        }

        [RelayCommand]
        async Task UpdateCarAsync(int id)
        {
            AddEditButtonText = editButtonText;
            return;
        }

        [RelayCommand]
        async Task SetEditMode(int id)
        {
            AddEditButtonText = editButtonText;
            CarId = id;
            var car = App.CarService.GetCar(id);
            Make = car.Make;
            Model = car.Model;
            Vin = car.Vin;
        }

        [RelayCommand]
        async Task ClearFormAsync()
        {
            AddEditButtonText = createButtonText;
            CarId = 0;
            Make = string.Empty;
            Model = string.Empty;
            Vin = string.Empty;
        }

        private async Task ShowAlert(string message)
        {
            await Shell.Current.DisplayAlert("Info", message, "Ok");
        }
    }
}
