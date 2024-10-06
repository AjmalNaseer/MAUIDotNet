using MoneyContribution.ViewModels;

namespace MoneyContribution.Views;

public partial class HistoryPage : ContentPage
{
    private HistoryVM _viewModel;

    public HistoryPage()
	{
		InitializeComponent();
        _viewModel = new HistoryVM();
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