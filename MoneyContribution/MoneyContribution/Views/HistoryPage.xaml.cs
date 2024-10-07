using MoneyContribution.ViewModels;

namespace MoneyContribution.Views;

public partial class HistoryPage : ContentPage
{
    public HistoryPage(HistoryVM viewmodel)
	{
		InitializeComponent();
        BindingContext = viewmodel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is HistoryVM viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
}