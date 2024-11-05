using ToDoList.ViewModels;

namespace ToDoList.Views;

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