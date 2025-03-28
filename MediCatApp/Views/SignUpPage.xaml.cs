namespace MediCatApp.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();

            // Attach event handlers from Tools
            SignUpBtn.Pressed += Tools.ButtonPressed;
            SignUpBtn.Released += Tools.ButtonReleased;
        }
        private async void OnImageTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StartPage());
        }
        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
        private async void OnCheckedChanged(object sender, EventArgs e)
        {
            if (DisplayPassword.IsChecked)
            {
                Password.IsPassword = false;
                RePassword.IsPassword = false;
            }
            else
            {
                Password.IsPassword = true;
                RePassword.IsPassword = true;
            }
        }
    }

}
