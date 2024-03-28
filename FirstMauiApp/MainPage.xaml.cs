using FirstMauiApp.ViewModels;

namespace FirstMauiApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(CarListViewModel carListViewModel)
        {
            InitializeComponent();
            BindingContext = carListViewModel;
        }
    }

}
