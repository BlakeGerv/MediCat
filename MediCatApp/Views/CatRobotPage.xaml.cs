using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System.Text;
using System.Text.Json;

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
        //Labels Text as variables
        [ObservableProperty]
        private string? _errormessage;

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
        [RelayCommand]
        private async Task PostRequest()
        {
            Errormessage = "";

            try
            {
                var payload = new
                {
                    command = "alarm",
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsync($"http://10.112.244.126/command", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Errormessage = e.Message;
            }
        }
    }
}
