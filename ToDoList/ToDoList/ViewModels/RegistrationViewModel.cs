using System.Windows.Input;
using ToDoList.Models;
using Microsoft.Maui.Controls;
using ToDoList.Services;
using ToDoList.Views;

namespace ToDoList.ViewModels
{
    public class RegistrationViewModel
    {
        private readonly DBService _dbService;
        public ICommand NavigateToSignInCommand { get; }
        public ICommand RegisterCommand { get; }


        public RegistrationViewModel()
        {
            _dbService = DBService.Instance; // Access singleton instance
            RegisterCommand = new Command(async () => await Register());
            NavigateToSignInCommand = new Command(async () => await NavigateToSignInPage());

        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }


        private async Task Register()
        {
            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            var user = new UserModel
            {
                FullName = FullName,
                Email = Email,
                Password = Password
            };

            await _dbService.SaveUserAsync(user);
            await Application.Current.MainPage.DisplayAlert("Success", "Registration completed", "OK");
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());

            // Navigate to the login or another page if needed
        }
        private async Task NavigateToSignInPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

    }
}
