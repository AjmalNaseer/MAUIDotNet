using ToDoList.ViewModels;
using ToDoList.Models;

namespace ToDoList.Views
{
    public partial class AllTaskPage : ContentPage
    {
        private AllTaskViewModel _viewModel;

        public AllTaskPage()
        {
            InitializeComponent();
            _viewModel = new AllTaskViewModel();
            BindingContext = _viewModel;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadTasksAsync();
        }

        // Handle item selection (navigate to a detailed task page)
        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedTask = e.SelectedItem as TodoItemModel;
                if (selectedTask != null)
                {
                    // Pass the selected task to the AddTaskViewModel
                    await Navigation.PushAsync(new AddTaskPage
                    {
                        BindingContext = new AddTaskViewModel(selectedTask)
                    });
                }

                // Deselect the item after navigating
                ((ListView)sender).SelectedItem = null;
            }
        }

    }
}
