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
        public string Email => _authClient.User?.Info?.Email;
        public string PhotoUrl => _authClient.User?.Info?.PhotoUrl;

        public EditProfilePageViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task NavigateMain()
        {
            await Shell.Current.GoToAsync("//Main");
        }
        [RelayCommand]
        private async Task NavigateProfile()
        {
            await Shell.Current.GoToAsync("//Profile");
        }
        [RelayCommand]
        private async Task RefreshPhotoUrl()
        {
            Random rand = new Random();
            _authClient.User.Info.PhotoUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{rand.Next(650)}.png";
            OnPropertyChanged(nameof(PhotoUrl));
        }
        [RelayCommand]
        private async Task UpdateProfile()
        {
            var IdToken = await _authClient.User.GetIdTokenAsync();
            
            var payload = new
            {
                idToken = IdToken,
                photoUrl = PhotoUrl,
                returnSecureToken = true
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:update?key={MauiProgram.apiKey}", content);
            response.EnsureSuccessStatusCode();

            OnPropertyChanged(nameof(PhotoUrl));

            await Shell.Current.GoToAsync("//Profile");
        }
    }
}
