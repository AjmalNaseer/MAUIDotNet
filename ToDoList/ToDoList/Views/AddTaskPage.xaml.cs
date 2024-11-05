using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Views;

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