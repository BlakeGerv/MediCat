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
    }

}
