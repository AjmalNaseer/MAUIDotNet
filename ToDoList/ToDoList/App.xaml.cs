using QuickList.Services;

namespace QuickList
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitializeDatabaseAsync();
            MainPage = new AppShell();  // Navigate to the main page after database initialization

        }
        private async void InitializeDatabaseAsync()
        {
            await DBService.Instance.InitializeAsync();
        }
    }
}
