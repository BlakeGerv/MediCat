using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace MediCatApp.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(ProfilePageViewModel viewModel)
        {
            InitializeComponent();

            // Attach event handlers from Tools
            if (Tools.ButtonPressed != null)
                EditProfileBtn.Pressed += Tools.ButtonPressed;
            if (Tools.ButtonReleased != null)
                EditProfileBtn.Released += Tools.ButtonReleased;
            if (Tools.ButtonPressed != null)
                LogOutBtn.Pressed += Tools.ButtonPressed;
            if (Tools.ButtonReleased != null)
                LogOutBtn.Released += Tools.ButtonReleased;

            BindingContext = viewModel;
        }
    }
    public partial class ProfilePageViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        public string Username => _authClient.User?.Info?.DisplayName;
        public string Email => _authClient.User?.Info?.Email;
        public string PhotoUrl => _authClient.User?.Info?.PhotoUrl;

        public ProfilePageViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task NavigateMain()
        {
            await Shell.Current.GoToAsync("//Main");
        }
        [RelayCommand]
        private async Task NavigateEditProfile()
        {
            await Shell.Current.GoToAsync("//EditProfile");
        }
    }
}
