using Microsoft.Extensions.Logging;
using ToDoList.Services;
using ToDoList.ViewModels;
using ToDoList.Views;

namespace ToDoList
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
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            //ViewModels
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<RegistrationViewModel>();
            builder.Services.AddSingleton<AllTaskViewModel>();
            builder.Services.AddSingleton<AddTaskViewModel>();
            builder.Services.AddSingleton<UserInfoViewModel>();
            builder.Services.AddSingleton<DBService>();
            

            //Pages
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<RegistrationPage>();
            builder.Services.AddSingleton<AllTaskPage>();
            builder.Services.AddSingleton<AddTaskPage>();
            builder.Services.AddSingleton<UserInfoPage>();
            
            return builder.Build();
        }
    }
}
