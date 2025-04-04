using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MediCatApp.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(ProfilePageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
    public partial class ProfilePageViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task NavigateMain()
        {
            await Shell.Current.GoToAsync("//Main");
        }
    }
}
