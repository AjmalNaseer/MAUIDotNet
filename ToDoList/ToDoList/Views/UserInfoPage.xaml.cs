using QuickList.ViewModels;

namespace QuickList.Views;

public partial class UserInfoPage : ContentPage
{
	public UserInfoViewModel _viewModel;
	public UserInfoPage()
	{
		InitializeComponent();
		_viewModel = new UserInfoViewModel();
		BindingContext = _viewModel;
	}
}