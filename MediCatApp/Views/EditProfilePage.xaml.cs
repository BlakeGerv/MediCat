using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Auth.Requests;
using System.Text;
using System.Text.Json;

namespace MediCatApp.Views
{
    public partial class EditProfilePage : ContentPage
    {
        public EditProfilePage(EditProfilePageViewModel viewModel)
        {
            InitializeComponent();

            // Attach event handlers from Tools
            if (Tools.ButtonPressed != null)
                ProfileBtn.Pressed += Tools.ButtonPressed;
            if (Tools.ButtonReleased != null)
                ProfileBtn.Released += Tools.ButtonReleased;

            BindingContext = viewModel;
        }
    }
    public partial class EditProfilePageViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        public string Username => _authClient.User?.Info?.DisplayName;
        [ObservableProperty]
        private string _newusername;
        public string Email => _authClient.User?.Info?.Email;
        public string PhotoUrl => _authClient.User?.Info?.PhotoUrl;
        [ObservableProperty]
        private string _newphotourl;

        [ObservableProperty]
        private string _usernamecheck;

        public EditProfilePageViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
            Usernamecheck = "";
            Newusername = Username;
            Newphotourl = PhotoUrl;
        }

        [RelayCommand]
        private async Task NavigateMain()
        {
            Newphotourl = PhotoUrl;
            Newusername = Username;
            await Shell.Current.GoToAsync("//Main");
        }
        [RelayCommand]
        private async Task NavigateProfile()
        {
            Newphotourl = PhotoUrl;
            Newusername = Username;
            await Shell.Current.GoToAsync("//Profile");
        }
        [RelayCommand]
        private async Task UpdateProfile()
        {
            Usernamecheck = "";

            //Username Validation
            if (!string.IsNullOrEmpty(Newusername) && Newusername.Length < 3)
            {
                Usernamecheck = "Username should be at least 3 characters";
                return;
            }

            if (string.IsNullOrEmpty(Newusername))
                Newusername = Username;

            if (string.IsNullOrEmpty(Newphotourl))
                Newphotourl = PhotoUrl;

            //Update PhotoUrl and Username in firebase
            var IdToken = await _authClient.User.GetIdTokenAsync();

            var payload = new
            {
                idToken = IdToken,
                photoUrl = Newphotourl,
                displayName = Newusername,
                returnSecureToken = true
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:update?key={MauiProgram.apiKey}", content);
            response.EnsureSuccessStatusCode();


            //Update PhotoUrl and Username in current
            _authClient.User.Info.PhotoUrl = Newphotourl;
            _authClient.User.Info.DisplayName = Newusername;

            OnPropertyChanged(nameof(PhotoUrl));
            OnPropertyChanged(nameof(Username));

            await Shell.Current.GoToAsync("//Profile");
        }
    }
}
