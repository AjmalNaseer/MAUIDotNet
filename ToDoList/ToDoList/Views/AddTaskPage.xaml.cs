using QuickList.Models;
using QuickList.ViewModels;

namespace QuickList.Views;

public partial class AddTaskPage : ContentPage
{
    public AddTaskViewModel _viewModel;
	public AddTaskPage()
	{
		InitializeComponent();
        _viewModel = new AddTaskViewModel();
        BindingContext = _viewModel;
    }

}