using MoneyContribution.ViewModels;

namespace MoneyContribution.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardVM viewmodel)
    {
        InitializeComponent();
        BindingContext = viewmodel;

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DashboardVM viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
}
