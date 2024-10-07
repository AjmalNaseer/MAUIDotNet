using MoneyContribution.ViewModels;

namespace MoneyContribution.Views;

public partial class ContributePage : ContentPage
{
    public ContributePage(ContributeVM viewmodel)
    {
        InitializeComponent();
        BindingContext = viewmodel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ContributeVM viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }

}