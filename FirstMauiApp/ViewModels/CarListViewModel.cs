﻿using CommunityToolkit.Mvvm.Input;
using FirstMauiApp.Models;
using FirstMauiApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FirstMauiApp.ViewModels
{
    public partial class CarListViewModel : BaseViewModel
    {
        private readonly CarService carService;
        public ObservableCollection<Car> Cars { get; private set; } = [];

        public CarListViewModel(CarService carService)
        {
            Title = "Car List";
            this.carService = carService;
        }

        [RelayCommand]
        async Task GetCarListAsync()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                if (Cars.Any()) Cars.Clear();

                var cars = carService.GetCars();
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
            }
        }
    }
}
