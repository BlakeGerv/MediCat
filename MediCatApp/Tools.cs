namespace MediCatApp
{
    public static class Tools
    {
        public static void ButtonPressed(object sender, EventArgs e)
        {
            if (sender is ImageButton imgbutton)
            {
                if (imgbutton.BackgroundColor == Color.FromArgb("#0e1111"))
                    imgbutton.BackgroundColor = Color.FromArgb("#232b2b");
            }
            else if (sender is Button button)
            {
                if (button.BackgroundColor == Colors.LightSteelBlue)
                    button.BackgroundColor = Colors.LightSlateGray;
            }
        }
        public static void ButtonReleased(object sender, EventArgs e)
        {
            if (sender is ImageButton imgbutton)
            {
                if (imgbutton.BackgroundColor == Color.FromArgb("#232b2b"))
                    imgbutton.BackgroundColor = Color.FromArgb("#0e1111");
            }
            else if (sender is Button button)
            {
                if (button.BackgroundColor == Colors.LightSlateGray)
                    button.BackgroundColor = Colors.LightSteelBlue;
            }
        }
    }
}
