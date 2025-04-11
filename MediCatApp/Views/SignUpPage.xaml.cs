using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System.Text;
using System.Text.Json;

namespace MediCatApp.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage(SignUpViewModel viewModel)
        {
            InitializeComponent();

            // Attach event handlers from Tools
            SignUpBtn.Pressed += Tools.ButtonPressed;
            SignUpBtn.Released += Tools.ButtonReleased;

            BindingContext = viewModel;
        }
        private async void OnCheckedChanged(object sender, EventArgs e)
        {
            if (DisplayPassword.IsChecked)
            {
                Password.IsPassword = false;
                RePassword.IsPassword = false;
            }
            else
            {
                Password.IsPassword = true;
                RePassword.IsPassword = true;
            }
        }
    }

    public partial class SignUpViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        //Entry Text as variables
        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _repassword;

        //Label Text as variables
        [ObservableProperty]
        private string _errormessage;

        [ObservableProperty]
        private string _emailcheck;

        [ObservableProperty]
        private string _usernamecheck;

        [ObservableProperty]
        private string _passwordcheck;

        [ObservableProperty]
        private string _repasswordcheck;

        public SignUpViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
            Emailcheck = Usernamecheck = Passwordcheck = Repasswordcheck = "*";
            Email = Username = Password = Repassword = Errormessage = "";
        }

        [RelayCommand]
        private async Task SignUp()
        {
            Emailcheck = Usernamecheck = Errormessage = "";
            Passwordcheck = Repasswordcheck = "*";

            bool hasError = false;

            //Username Validation
            if (string.IsNullOrEmpty(Username))
            {
                Usernamecheck = "Missing Required Element";
                hasError = true;
            }
            else if (Username.Length < 6)
            {
                Usernamecheck = "Username should be at least 6 characters";
                hasError = true;
            }

            //Password Validation
            if (string.IsNullOrEmpty(Password))
            {
                Passwordcheck = "Missing Required Element";
                hasError = true;
            }
            else if (Password.Length < 6)
            {
                Passwordcheck = "Password should be at least 6 characters";
                hasError = true;
            }

            //Re-Password Validation
            if (string.IsNullOrEmpty(Repassword))
            {
                Repasswordcheck = "Missing Required Element";
                hasError = true;
            }
            else if (string.IsNullOrEmpty(Password) || Password != Repassword)
            {
                Repasswordcheck = "Passwords must match";
                hasError = true;
            }

            //Email Validation
            if (string.IsNullOrEmpty(Email))
            {
                Emailcheck = "Missing Required Element";
                hasError = true;
            }

            //Early return before firebase call if error is found
            if (hasError)
            {
                Password = Repassword = "";
                return;
            }

            try
            {
                await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password, Username);
                try
                {
                    Random rand = new Random();
                    _authClient.User.Info.PhotoUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{rand.Next(650)}.png";

                    var IdToken = await _authClient.User.GetIdTokenAsync();

                    var payload = new
                    {
                        idToken = IdToken,
                        photoUrl = _authClient.User.Info.PhotoUrl,
                        returnSecureToken = true
                    };

                    var jsonPayload = JsonSerializer.Serialize(payload);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    using var httpClient = new HttpClient();
                    var response = await httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:update?key={MauiProgram.apiKey}", content);
                    response.EnsureSuccessStatusCode();

                    Email = Username = Password = Repassword = "";
                    await Shell.Current.GoToAsync("//Login");
                }
                catch (Exception e)
                {
                    Errormessage = e.Message;
                }
            }
            catch (FirebaseAuthHttpException e)
            {
                if (e.ResponseData.Contains("INVALID_EMAIL"))
                    Emailcheck = "Invalid Email";
                else if (e.ResponseData.Contains("EMAIL_EXISTS"))
                    Emailcheck = "Email Exists";
                else
                    Errormessage = e.ResponseData;

                Password = Repassword = "";
            }

        }
        [RelayCommand]
        private async Task NavigateLogin()
        {
            Email = Username = Password = Repassword = Errormessage = "";
            Emailcheck = Usernamecheck = Passwordcheck = Repasswordcheck = "*";
            await Shell.Current.GoToAsync("//Login");
        }
    }
}
