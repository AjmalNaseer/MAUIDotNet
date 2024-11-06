using QuickList.ViewModels;

namespace QuickList.Views;

public partial class RegistrationPage : ContentPage
{
    private RegistrationViewModel _viewModel;

    public RegistrationPage()
	{
		InitializeComponent();
        _viewModel = new RegistrationViewModel();

        BindingContext = _viewModel;
    }
}