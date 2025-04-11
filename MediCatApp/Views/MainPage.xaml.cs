using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace MediCatApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        public string? PhotoUrl => _authClient.User?.Info?.PhotoUrl;

        public MainPageViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task NavigateProfile()
        {
            await Shell.Current.GoToAsync("//Profile");
        }
        [RelayCommand]
        private async Task NavigateCatRobot()
        {
            await Shell.Current.GoToAsync("//CatRobot");
        }
        [RelayCommand]
        private async Task NavigateWeightSensor()
        {
            await Shell.Current.GoToAsync("//WeightSensor");
        }
    }
}
