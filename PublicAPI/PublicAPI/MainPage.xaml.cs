using PublicAPI.ViewModels;

namespace PublicAPI
{
    public partial class MainPage : ContentPage
    {
        public MainPageVM mainPageVM;

        public MainPage()
        {
            InitializeComponent();

            // Initialize ViewModel and set BindingContext
            mainPageVM = new MainPageVM();
            BindingContext = mainPageVM;

            // Call LoadData() after ViewModel initialization
            _ = mainPageVM.LoadData();
        }
    }
}
