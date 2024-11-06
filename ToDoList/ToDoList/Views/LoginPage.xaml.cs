using QuickList.ViewModels;

namespace QuickList.Views
{
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();

            _viewModel = new LoginViewModel();
            BindingContext = _viewModel;
        }
    }
}
