using MoneyContribution.ViewModels;

namespace MoneyContribution.Views;

public partial class ContributePage : ContentPage
{
    public ContributePage(ContributeVM viewmodel)
    {
        InitializeComponent();
        BindingContext = viewmodel;
    }
}