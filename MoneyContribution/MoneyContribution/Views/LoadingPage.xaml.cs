using MoneyContribution.ViewModels;

namespace MoneyContribution.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoadingPageVM viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;

    }
}