using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.Logging;
using MoneyContribution.Handlers;
using MoneyContribution.Services;
using MoneyContribution.ViewModels;
using MoneyContribution.Views;

namespace MoneyContribution
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Inter_18pt-Bold.ttf", "InterBold");
                    fonts.AddFont("Inter_18pt-Regular.ttf", "InterRegular");
                    fonts.AddFont("Inter_18pt-SemiBold.ttf", "InterSemibold");
                });
           

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = ContribAPIs.ApiKey,
                AuthDomain = ContribAPIs.AuthDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }

            }));
            // ViewModels
            builder.Services.AddSingleton<LoginVM>();
            builder.Services.AddSingleton<RegisterationVM>();
            builder.Services.AddSingleton<DashboardVM>();
            builder.Services.AddSingleton<SettingsVM>();
            builder.Services.AddSingleton<HistoryVM>();
            builder.Services.AddSingleton<ContributeVM>();
            builder.Services.AddSingleton<LoadingPageVM>();
            builder.Services.AddSingleton<AppShellVM>();


            //Pages
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddSingleton<ContributePage>();
            builder.Services.AddSingleton<HistoryPage>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<LoadingPage>();
            

            return builder.Build();
        }
    }
}
