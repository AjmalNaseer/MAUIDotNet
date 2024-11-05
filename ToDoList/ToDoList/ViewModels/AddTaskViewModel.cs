using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.Views;

namespace ToDoList.ViewModels
{
    public class AddTaskViewModel : BaseViewModel
    {
        private TodoItemModel _todoItem;
        private readonly DBService _dbService;

        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
        // Properties
        public TodoItemModel TodoItem
        {
            get => _todoItem;
            set => SetProperty(ref _todoItem, value);
        }

        private UserModel _currentUser;
        public string FullName => _currentUser?.FullName;
        public string Email => _currentUser?.Email;

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand NavigateBackCommand { get; }

        // Constructors
        public AddTaskViewModel(TodoItemModel todoItem = null)
        {
            _dbService = DBService.Instance; // Access singleton instance
            TodoItem = todoItem ?? new TodoItemModel(); // Initialize TodoItem if null

            Name = TodoItem.Name;
            Notes = TodoItem.Notes;
            Done = TodoItem.Done;
            // Initialize Commands
            SaveCommand = new Command(async () => await SaveTask());
            NavigateBackCommand = new Command(async () => await NavigateBack());
            DeleteCommand = new Command(async () => await DeleteTask());
            CancelCommand = new Command(async () => await CancelTask());

            // Load current user data with error handling
            Task.Run(async () =>
            {
                try
                {
                    await LoadCurrentUser();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that might occur during the user load process
                    Console.WriteLine(ex.Message);
                }
            }).ConfigureAwait(false);
        }


        // Load the current user
        private async Task LoadCurrentUser()
        {
            _currentUser = await _dbService.GetCurrentUser();
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Email));
        }

        // Save Task (Add or Update)
        private async Task SaveTask()
        {
            if (TodoItem == null) return;

            // Save the item (new or update)
            await _dbService.SaveItemAsync(TodoItem);

            // Show success alert
            await ShowAlert("Success", TodoItem.ID == 0 ? "Task Added" : "Task Updated");
            await Application.Current.MainPage.Navigation.PushAsync(new AllTaskPage());
        }

        private async Task DeleteTask()
        {
            if (TodoItem == null) return;

            // Delete the task
            await _dbService.DeleteItemAsync(TodoItem);

            // Show success alert
            await ShowAlert("Success", "Task Deleted");

            // Ensure page is still valid before navigating
            await Application.Current.MainPage.Navigation.PushAsync(new AllTaskPage());

        }

        private async Task CancelTask()
        {
            // Ensure page is still valid before navigating
            await Application.Current.MainPage.Navigation.PushAsync(new AllTaskPage());
        }


        // Helper for navigation
        private async Task NavigateBack()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AllTaskPage());
        }

        // Display Alert
        private async Task ShowAlert(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}
