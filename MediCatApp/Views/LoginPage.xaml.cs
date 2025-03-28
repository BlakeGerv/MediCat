namespace MediCatApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            // Attach event handlers from Tools
            if (Tools.ButtonPressed != null)
                LoginBtn.Pressed += Tools.ButtonPressed;
            if (Tools.ButtonReleased != null)
                LoginBtn.Released += Tools.ButtonReleased;
        }
        private async void OnSignUpTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
        private async void OnCheckedChanged(object sender, EventArgs e)
        {
            if (DisplayPassword.IsChecked)
                Password.IsPassword = false;
            else
                Password.IsPassword = true;
        }
    }

}
