using MoneyContribution.ViewModels;

namespace MoneyContribution.Views;
public partial class LoginPage : ContentPage
{
    public LoginPage(LoginVM viewmodel)
    {
        InitializeComponent();
        BindingContext = viewmodel;

    }
}
