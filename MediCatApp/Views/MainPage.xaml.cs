using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MediCatApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
    public partial class MainPageViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task NavigateProfile()
        {
            await Shell.Current.GoToAsync("//Profile");
        }
    }
}
