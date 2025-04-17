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

        //Hex codes for border and images
        public readonly string[] hex = ["ff3e3e", "6eff89", "00a5ff", "b66eff", "ff6ed3"];

        //Image and border colours
        [ObservableProperty]
        public Color _catBorderColour;
        [ObservableProperty]
        public Color _pillBorderColour;
        [ObservableProperty]
        public string _catIconSource;
        [ObservableProperty]
        public string _pillIconSource;

        //Cat and Pill ON/OFF
        [ObservableProperty]
        public Color _catOnOffColour;
        [ObservableProperty]
        public string _catOnOffText;
        [ObservableProperty]
        public Color _pillOnOffColour;
        [ObservableProperty]
        public string _pillOnOffText;

        //Cat and Pill device names
        [ObservableProperty]
        public string _catName;
        [ObservableProperty]
        public string _pillName;

        //ip addresses
        [ObservableProperty]
        public string _catRobotIP;
        [ObservableProperty]
        public string _weightSensorIP;

        public MainPageViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
            Tools.CatBorderColour = Color.FromArgb(hex[0]);
            CatBorderColour = Tools.CatBorderColour;
            CatIconSource = $"cat_icon_{hex[0]}.png";
            Tools.PillBorderColour = Color.FromArgb(hex[2]);
            PillBorderColour = Tools.PillBorderColour;
            PillIconSource = $"pill_icon_{hex[2]}.png";

            Tools.CatName = "Cat Robot";
            CatName = Tools.CatName;
            Tools.PillName = "Weight Sensor";
            PillName = Tools.PillName;

            PillOnOffColour = Colors.Tomato;
            PillOnOffText = "Off";
            CatOnOffColour = Colors.Tomato;
            CatOnOffText = "Off";

            CatRobotIP = "http://192.168.129.37";
            WeightSensorIP = "http://192.168.129.213";
            _ = CheckIfOn(CatRobotIP);
            _ = CheckIfOn(WeightSensorIP);
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
            CatName = Tools.CatName;
            _ = CheckIfOn(CatRobotIP);
            await Shell.Current.GoToAsync("//CatRobot");
        }
        [RelayCommand]
        private async Task NavigateWeightSensor()
        {
            OnPropertyChanged(nameof(PhotoUrl));
            PillName = Tools.PillName;
            _ = CheckIfOn(WeightSensorIP);
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
                    Tools.PillBorderColour = Color.FromArgb(tHex);
                    PillBorderColour = Tools.PillBorderColour;
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
                    Tools.CatBorderColour = Color.FromArgb(tHex);
                    CatBorderColour = Tools.CatBorderColour;
                    CatIconSource = $"cat_icon_{tHex}.png";
                    return;
                }
            }
        }
        [RelayCommand]
        private async Task CheckIfOn(string address)
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(address);
                response.EnsureSuccessStatusCode();

                if (address == WeightSensorIP)
                {
                    PillOnOffColour = Colors.Green;
                    PillOnOffText = "On";
                }
                else if (address == CatRobotIP)
                {
                    CatOnOffColour = Colors.Green;
                    CatOnOffText = "On";
                }
            }
            catch
            {
                if (address == WeightSensorIP)
                {
                    PillOnOffColour = Colors.Tomato;
                    PillOnOffText = "Off";
                }
                else if (address == CatRobotIP)
                {
                    CatOnOffColour = Colors.Tomato;
                    CatOnOffText = "Off";
                }
            }
        }
    }
}
