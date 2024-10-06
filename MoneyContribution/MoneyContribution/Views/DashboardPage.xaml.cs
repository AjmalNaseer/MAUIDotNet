using MoneyContribution.ViewModels;

namespace MoneyContribution.Views;

public partial class DashboardPage : ContentPage
{
    private DashboardVM _viewModel;

    public DashboardPage()
    {
        InitializeComponent();
        _viewModel = new DashboardVM();
        BindingContext = _viewModel; 
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null)
        {
            _viewModel.RefreshData();
        }
    }
}
