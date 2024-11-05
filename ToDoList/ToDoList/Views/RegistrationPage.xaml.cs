using ToDoList.ViewModels;

namespace ToDoList.Views;

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