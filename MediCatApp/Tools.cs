using CommunityToolkit.Mvvm.ComponentModel;

namespace MediCatApp
{
    public static class Tools
    {
        public static Color? CatBorderColour;
        public static Color? PillBorderColour;
        public static string? CatName;
        public static string? PillName;

        public static void ButtonPressed(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.BackgroundColor == Colors.LightSteelBlue)
                    button.BackgroundColor = Colors.LightSlateGray;
                else if (button.BackgroundColor == Colors.DimGrey)
                    button.BackgroundColor = Colors.Black;
            }
        }
        public static void ButtonReleased(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.BackgroundColor == Colors.LightSlateGray)
                    button.BackgroundColor = Colors.LightSteelBlue;
                else if (button.BackgroundColor == Colors.Black)
                    button.BackgroundColor = Colors.DimGrey;
            }
        }
    }
}
