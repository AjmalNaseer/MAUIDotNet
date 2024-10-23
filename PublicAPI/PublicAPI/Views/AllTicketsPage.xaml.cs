using PublicAPI.ViewModels;

namespace PublicAPI.Views;

public partial class AllTicketsPage : ContentPage
{
    public AllTicketsVM _viewModel;
    public AllTicketsPage()
	{
		InitializeComponent();
        _viewModel = new AllTicketsVM();
        BindingContext = _viewModel;
    }
}