using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

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
        private readonly FirebaseAuthClient _authClient;

        public StartViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }
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
