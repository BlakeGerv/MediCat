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

            // Attach event handlers from Tools
            if (Tools.ButtonPressed != null)
                CalibrateBtn.Pressed += Tools.ButtonPressed;
            if (Tools.ButtonReleased != null)
                CalibrateBtn.Released += Tools.ButtonReleased;

            BindingContext = viewModel;
        }
    }
    public partial class WeightSensorPageViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        public string PhotoUrl => _authClient.User?.Info?.PhotoUrl;
        [ObservableProperty]
        private string _webserverdata;
        [ObservableProperty]
        private int _numOfPills;

        //Labels Text as variables
        [ObservableProperty]
        private string? _errormessage;
        [ObservableProperty]
        private string? _calibrationmessage;

        //Button Text as a variable
        [ObservableProperty]
        private string? _calibratebuttontext;

        //Calibration Data as variables
        [ObservableProperty]
        private double _weightOfBottle;
        [ObservableProperty]
        private double _weightOfPills;
        [ObservableProperty]
        private int _calibrationStage;

        public WeightSensorPageViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;

            CalibrationStage = 0;
            Calibratebuttontext = "Begin Calibration";

            WeightOfBottle = -100;
            WeightOfPills = 0;

            Webserverdata = "<html>[...]Weight: 0 units[...]</html>";
        }
        [RelayCommand]
        private async Task ResetPage()
        {
            CalibrationStage = 0;
            Calibratebuttontext = "Begin Calibration";
            Calibrationmessage = "";
            Errormessage = "";
            OnPropertyChanged(nameof(PhotoUrl));
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
        private async Task<string> ReadFromWebserver()
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"http://192.168.129.213");
                response.EnsureSuccessStatusCode();

                Webserverdata = await response.Content.ReadAsStringAsync();
                return Webserverdata;
            }
            catch (Exception e)
            {
                Errormessage = e.Message;
                return Errormessage;
            }
        }
        [RelayCommand]
        private async Task<double> GetWeightOnSensor()
        {
            //await ReadFromWebserver();

            double WeightOnSensor = -1000;

            int startIndex = Webserverdata.IndexOf("Weight: ") + 8;
            int endIndex = Webserverdata.IndexOf(" units", startIndex);

            if (startIndex != -1 && endIndex != -1)
            {
                string numberString = Webserverdata.Substring(startIndex, endIndex - (startIndex));
                if (double.TryParse(numberString, out double weight))
                    WeightOnSensor = weight;
                else
                    Errormessage = "Failed to parse the number.";
            }
            else
                Errormessage = "Tags not found.";

            return WeightOnSensor;
        }
        [RelayCommand]
        private async Task Calibrate()
        {
            if (CalibrationStage == 0)
            {
                Calibratebuttontext = "Continue Calibration";
                Calibrationmessage = "Place EMPTY PILL BOTTLE on sensor.";
                CalibrationStage++;
            }
            else if (CalibrationStage > 0 && CalibrationStage < 4)
            {
                Calibratebuttontext = "Continue Calibration";
                Calibrationmessage = $"Place PILL BOTTLE and {CalibrationStage} PILL on sensor.";
                if (CalibrationStage == 1)
                {
                    WeightOfBottle = await GetWeightOnSensor();
                    WeightOfPills = 0;
                }
                else
                    WeightOfPills += await GetWeightOnSensor();
                CalibrationStage++;
            }
            else if (CalibrationStage == 4)
            {
                WeightOfPills += await GetWeightOnSensor();
                WeightOfPills = (WeightOfPills - (WeightOfBottle * 3)) / 6;

                Calibratebuttontext = "Calibration Complete";
                Calibrationmessage = "";
                CalibrationStage++;
            }
            else if (CalibrationStage != 5)
            {
                Calibratebuttontext = "Begin Calibration";
                Calibrationmessage = "";
                CalibrationStage = 0;
            }
        }
        [RelayCommand]
        private async Task<int> RefreshPillCount()
        {
            if (CalibrationStage != 0 && CalibrationStage != 5)
            {
                Errormessage = "Calibration incomplete.";
            }
            else if (WeightOfBottle == -100 || WeightOfPills == 0)
            {
                Errormessage = "Must complete calibration first.";
            }

            double WeightOnSensor = await GetWeightOnSensor();

            NumOfPills = (int)Math.Round((WeightOnSensor - WeightOfBottle) / WeightOfPills);

            return NumOfPills;
        }
    }
}
