using System.ComponentModel;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Services;
using Microsoft.Maui.Controls;
using ToDoList.Views;
using System.Windows.Input;  // For MainThread

namespace ToDoList.ViewModels
{
    public class UserInfoViewModel : INotifyPropertyChanged
    {
        private readonly DBService _dbService;
        private UserModel _currentUser;

        // Properties to bind user information to the UI
        public string FullName { get => _currentUser?.FullName; set { _currentUser.FullName = value; OnPropertyChanged(); } }
        public string Email { get => _currentUser?.Email; set { _currentUser.Email = value; OnPropertyChanged(); } }
        public string Address { get => _currentUser?.Address; set { _currentUser.Address = value; OnPropertyChanged(); } }
        public string PhoneNumber { get => _currentUser?.PhoneNumber; set { _currentUser.PhoneNumber = value; OnPropertyChanged(); } }

        // Command to save user information
        public Command SaveCommand { get; }
        public ICommand NavigateBackCommand { get; }

        // Constructor
        public UserInfoViewModel()
        {
            _dbService = DBService.Instance;
            SaveCommand = new Command(async () => await SaveUserInfo()); // Bind SaveCommand to SaveUserInfo method
            Task.Run(async () => await LoadCurrentUser()).ConfigureAwait(false);
            NavigateBackCommand = new Command(async () => await NavigateBack());

        }

        // Load the current user from the database or authentication service
        private async Task LoadCurrentUser()
        {
            _currentUser = await _dbService.GetCurrentUser(); // Fetch user from the DB or service
            OnPropertyChanged(nameof(FullName));  // Notify UI of updated properties
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(PhoneNumber));
        }

        // Save the updated user information to the database
        private async Task SaveUserInfo()
        {
            if (_currentUser == null)
                return;

            // Save the updated user information to the database
            await _dbService.SaveUserAsync(_currentUser);

            // Optionally, show a success message
            await Application.Current.MainPage.DisplayAlert("Success", "Your information has been updated.", "OK");

            // Optionally, navigate back after saving
            await Application.Current.MainPage.Navigation.PushAsync(new AllTaskPage());
        }

        private async Task NavigateBack()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AllTaskPage());
        }

        // INotifyPropertyChanged implementation to notify UI of changes
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
