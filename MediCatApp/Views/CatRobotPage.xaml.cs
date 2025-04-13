using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace MediCatApp.Views
{
    public partial class CatRobotPage : ContentPage
    {
        public CatRobotPage(CatRobotPageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
    public partial class CatRobotPageViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        public string PhotoUrl => _authClient.User?.Info?.PhotoUrl;

        public CatRobotPageViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }
        [RelayCommand]
        private async Task NavigateMain()
        {
            OnPropertyChanged(nameof(PhotoUrl));
            await Shell.Current.GoToAsync("//Main");
        }
        [RelayCommand]
        private async Task NavigateProfile()
        {
            OnPropertyChanged(nameof(PhotoUrl));
            await Shell.Current.GoToAsync("//Profile");
        }
    }
}
