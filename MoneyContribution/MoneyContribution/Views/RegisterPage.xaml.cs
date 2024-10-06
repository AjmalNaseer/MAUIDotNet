using MoneyContribution.ViewModels;

namespace MoneyContribution.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterationVM viewmodel)
	{
		InitializeComponent();
        BindingContext = viewmodel;

    }
}