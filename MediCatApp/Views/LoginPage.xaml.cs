using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace MediCatApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(SignInViewModel viewModel)
        {
            InitializeComponent();

            // Attach event handlers from Tools
            if (Tools.ButtonPressed != null)
                LoginBtn.Pressed += Tools.ButtonPressed;
            if (Tools.ButtonReleased != null)
                LoginBtn.Released += Tools.ButtonReleased;

            BindingContext = viewModel;
        }
        private async void OnCheckedChanged(object sender, EventArgs e)
        {
            if (DisplayPassword.IsChecked)
                Password.IsPassword = false;
            else
                Password.IsPassword = true;
        }
    }

    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;
        public SignInViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task SignIn()
        {
            await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);
        }
        [RelayCommand]
        private async Task NavigateSignUp()
        {
            await Shell.Current.GoToAsync("//SignUp");
        }
    }
}
