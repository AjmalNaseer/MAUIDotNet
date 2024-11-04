using Microsoft.Maui.Controls;

namespace PublicAPI.Custom;

public class MyCustomClass : Label
{
        public MyCustomClass()
    {
        this.SizeChanged += OnSizeChanged;
        UpdateFontSize(); // Initial call to set font size
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        UpdateFontSize();
    }

    private void UpdateFontSize()
    {
        double newSize = CalculateFontSize();
        this.FontSize = newSize;
    }

    private double CalculateFontSize()
    {
        double width = Application.Current.MainPage.Width;
        double height = Application.Current.MainPage.Height;

        // Define the scaling logic
        if (width <= 1296 && height <= 960.75)
            return 15;
        else if (width > 1296 && height > 960.75)
            return 20;
        else
        {
            // Linear interpolation between the two sizes
            double scaleFactor = (width - 800) / (1024 - 800);
            return 15 + (scaleFactor * (20 - 15));
        }
    }
}

