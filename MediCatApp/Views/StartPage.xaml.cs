namespace MediCatApp.Views
{
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();

            // Attach event handlers from Tools
            LoginBtn.Pressed += Tools.ButtonPressed;
            LoginBtn.Released += Tools.ButtonReleased;
        }
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
        private async void OnSignUpTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }

}
