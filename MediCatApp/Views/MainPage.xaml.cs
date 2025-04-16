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

        public readonly string[] hex = ["ff3e3e", "6eff89", "00a5ff", "b66eff", "ff6ed3"];
        [ObservableProperty]
        public Color _catBorderColour;
        [ObservableProperty]
        public string _catIconSource;
        [ObservableProperty]
        public Color _pillBorderColour;
        [ObservableProperty]
        public string _pillIconSource;

        public MainPageViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
            CatBorderColour = Color.FromArgb(hex[0]);
            CatIconSource = $"cat_icon_{hex[0]}.png";
            PillBorderColour = Color.FromArgb(hex[2]);
            PillIconSource = $"pill_icon_{hex[2]}.png";
        }

        [RelayCommand]
        private async Task NavigateProfile()
        {
            OnPropertyChanged(nameof(PhotoUrl));
            await Shell.Current.GoToAsync("//Profile");
        }
        [RelayCommand]
        private async Task NavigateCatRobot()
        {
            OnPropertyChanged(nameof(PhotoUrl));
            await Shell.Current.GoToAsync("//CatRobot");
        }
        [RelayCommand]
        private async Task NavigateWeightSensor()
        {
            OnPropertyChanged(nameof(PhotoUrl));
            await Shell.Current.GoToAsync("//WeightSensor");
        }
        [RelayCommand]
        private async Task ChangePillColour()
        {
            for (int i = 0; i < 5; i++)
            {
                if (PillIconSource == $"pill_icon_{hex[i]}.png")
                {
                    string tHex = hex[(i < 4) ? i + 1 : 0];
                    PillBorderColour = Color.FromArgb(tHex);
                    PillIconSource = $"pill_icon_{tHex}.png";
                    return;
                }
            }
        }
        [RelayCommand]
        private async Task ChangeCatColour()
        {
            for (int i = 0; i < 5; i++)
            {
                if (CatIconSource == $"cat_icon_{hex[i]}.png")
                {
                    string tHex = hex[(i < 4) ? i + 1 : 0];
                    CatBorderColour = Color.FromArgb(tHex);
                    CatIconSource = $"cat_icon_{tHex}.png";
                    return;
                }
            }
        }
    }
}
