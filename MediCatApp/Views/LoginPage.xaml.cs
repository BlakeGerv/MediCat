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

        //Entry Text as variables
        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        //Label Text as variables
        [ObservableProperty]
        private string _errormessage;

        [ObservableProperty]
        private string _emailcheck;

        [ObservableProperty]
        private string _passwordcheck;

        public SignInViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
            Email = Password = Emailcheck = Passwordcheck = Errormessage = "";
        }

        [RelayCommand]
        private async Task SignIn()
        {
            Emailcheck = Passwordcheck = Errormessage = "";

            bool hasError = false;

            //Password Validation
            if (string.IsNullOrEmpty(Password))
            {
                Passwordcheck = "Missing Password";
                hasError = true;
            }

            //Email Validation
            if (string.IsNullOrEmpty(Email))
            {
                Emailcheck = "Missing Email";
                hasError = true;
            }

            //Early return before firebase call if error is found
            if (hasError)
            {
                Password = "";
                return;
            }

            try
            {
                await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);
                Email = Password = "";

                await Shell.Current.GoToAsync("//Main");
            }
            catch (FirebaseAuthHttpException e)
            {
                if (e.ResponseData.Contains("INVALID_EMAIL"))
                    Emailcheck = "Invalid Email";
                else if (e.ResponseData.Contains("INVALID_LOGIN_CREDENTIALS"))
                    Errormessage = "Invalid Login Credentials";
                else
                    Errormessage = e.ResponseData;

                Password = "";
            }
        }
        [RelayCommand]
        private async Task NavigateSignUp()
        {
            Email = Password = Emailcheck = Passwordcheck = Errormessage = "";
            await Shell.Current.GoToAsync("//SignUp");
        }
        [RelayCommand]
        private async Task NavigateMain()
        {
            await Shell.Current.GoToAsync("//Main");
        }
    }
}
