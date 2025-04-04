using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MediCatApp.Views
{
    public partial class StartPage : ContentPage
    {
        public StartPage(StartViewModel viewModel)
        {
            InitializeComponent();

            // Attach event handlers from Tools
            LoginBtn.Pressed += Tools.ButtonPressed;
            LoginBtn.Released += Tools.ButtonReleased;

            BindingContext = viewModel;
        }
    }

    public partial class StartViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task NavigateLogin()
        {
            await Shell.Current.GoToAsync("//Login");
        }
        [RelayCommand]
        private async Task NavigateSignUp()
        {
            await Shell.Current.GoToAsync("//SignUp");
        }
    }
}
