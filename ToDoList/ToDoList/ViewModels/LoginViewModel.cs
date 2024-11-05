using System;
using System.ComponentModel;
using System.Windows.Input;
using ToDoList.Services;
using ToDoList.Views;

namespace ToDoList.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly DBService _dbService;

        public ICommand NavigateToSignupCommand { get; }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _dbService = DBService.Instance; // Access singleton instance

            LoginCommand = new Command(async () => await Login());
            NavigateToSignupCommand = new Command(async () => await NavigateToSignupPage());

        }

        private async Task Login()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email and Password are required.", "OK");

                return;
            }
            var user = await _dbService.GetUserByEmailAndPasswordAsync(Email, Password);

            if (user != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AllTaskPage());
            }
            else
            {
                Console.WriteLine("Invalid email or password.");
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid email or password.", "OK");

            }
        }

        private async Task NavigateToSignupPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage());
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
