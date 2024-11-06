using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using QuickList.Models;
using QuickList.Services;
using QuickList.Views;
using Microsoft.Maui.Controls;

namespace QuickList.ViewModels
{
    public class AllTaskViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TodoItemModel> _tasks;
        private DBService _dbService;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TodoItemModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        private UserModel _currentUser;
        public string FullName => _currentUser?.FullName;
        public string Email => _currentUser?.Email;

        public ICommand OnSelectedItemCommand { get; }
        public ICommand UpdateProfileCommand { get; }
        public ICommand AddTaskCommand { get; }

        public AllTaskViewModel()
        {
            _dbService = DBService.Instance;
            OnSelectedItemCommand = new Command<TodoItemModel>(async (task) => await OnTaskSelected(task));
            UpdateProfileCommand = new Command(async () => await UpdateProfile());
            AddTaskCommand = new Command(async () => await AddTaskAsync());

            // Load current user and tasks asynchronously when ViewModel is initialized
            Task.Run(async () => await LoadCurrentUser()).ConfigureAwait(false);
            LoadTasksAsync(); // Load tasks
        }

        private async Task UpdateProfile()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new UserInfoPage());
        }
        // Load tasks from the database
        public async Task LoadTasksAsync()
        {
            // Ensure the page is part of the navigation stack before performing any actions
            if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
            {
                var tasks = await _dbService.GetItemsAsync();  // Retrieve tasks from the database
                Tasks = new ObservableCollection<TodoItemModel>(tasks);  // Bind tasks to ObservableCollection
            }
        }

        public async Task LoadCurrentUser()
        {
            // Fetch the current user from the database or auth service
            _currentUser = await _dbService.GetCurrentUser();  // Use 'await' to resolve the task
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Email));
        }

        private async Task OnTaskSelected(TodoItemModel task)
        {
            if (task == null)
                return;

            // Navigate to the task details page, passing the selected task as the BindingContext
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new AddTaskPage
                    {
                        BindingContext = task
                    });
                }
            });
        }

        private async Task AddTaskAsync()
        {
            // Navigate to AddTaskPage for creating a new task
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new AddTaskPage());
                }
            });
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
