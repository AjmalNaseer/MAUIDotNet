#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif
namespace PublicAPI
{
    public partial class App : Application
    {
        public App()
        {
            
            InitializeComponent();
            MainPage = new AppShell();
           
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            const double newWidth = 1296;
            const double newHeight = 960.75;

            window.X = 100;
            window.Y = 100;

            window.Width = newWidth;
            window.MinimumWidth = newWidth;
            window.Height = newHeight;
            window.MinimumHeight = newHeight;
            return window;
        }
        
    }
}
