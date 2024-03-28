using CommunityToolkit.Mvvm.ComponentModel;

namespace FirstMauiApp.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        string title;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading))]
        bool isLoading;


        public bool IsNotLoading => !isLoading;
    }
}
