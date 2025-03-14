namespace MediCatApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            // Attach event handlers from Tools
            LoginBtn.Pressed += Tools.ButtonPressed;
            LoginBtn.Released += Tools.ButtonReleased;
        }
        private async void OnImageTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StartPage());
        }
    }

}
