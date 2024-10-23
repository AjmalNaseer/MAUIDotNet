using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace PublicAPI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("RobotoCondensed-Regular.ttf", "RobotoCondensedRegular");
                    fonts.AddFont("Inter_28pt-SemiBold.ttf", "InterSemiBold");
                    
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
