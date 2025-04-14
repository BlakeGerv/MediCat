using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace MediCatApp.Views
{
    public partial class WeightSensorPage : ContentPage
    {
        public WeightSensorPage(WeightSensorPageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
    public partial class WeightSensorPageViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        public string PhotoUrl => _authClient.User?.Info?.PhotoUrl;
        [ObservableProperty]
        private string _webserverdata;

        public WeightSensorPageViewModel(FirebaseAuthClient authClient)
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
        [RelayCommand]
        private async Task<string> ReadFromWebserver()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://192.168.129.213"); //splice for "Weight: " and " units"
            response.EnsureSuccessStatusCode();

            Webserverdata = await response.Content.ReadAsStringAsync();

            return Webserverdata;
        }
    }
}
