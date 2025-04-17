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

        [ObservableProperty]
        public Color _catBorderColour;

        [ObservableProperty]
        public bool _catNameReadOnly;
        [ObservableProperty]
        public string _catName;

        public CatRobotPageViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;

            CatBorderColour = Tools.CatBorderColour;
            CatName = Tools.CatName;
            CatNameReadOnly = true;
        }
        [RelayCommand]
        private async Task ResetPage()
        {
            Errormessage = "";
            OnPropertyChanged(nameof(PhotoUrl));
            CatBorderColour = Tools.CatBorderColour;
        }
        [RelayCommand]
        private async Task NavigateMain()
        {
            ResetPage();
            await Shell.Current.GoToAsync("//Main");
        }
        [RelayCommand]
        private async Task NavigateProfile()
        {
            ResetPage();
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
                var response = await httpClient.PostAsync($"http://192.168.129.37:5000/command", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Errormessage = e.Message;
            }
        }
        [RelayCommand]
        private async Task SetNameEdit()
        {
            if (CatNameReadOnly)
                CatNameReadOnly = false;
            else
            {
                CatNameReadOnly = true;
                Tools.CatName = CatName;
            }
        }
    }
}
