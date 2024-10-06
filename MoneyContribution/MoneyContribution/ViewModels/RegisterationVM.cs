using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Auth.Providers;
using MoneyContribution.Models;
using MoneyContribution.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyContribution.ViewModels
{
    public partial class RegisterationVM :  ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private UserModel _user = new();

        public RegisterationVM(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task Register()
        {
            // Validate Email
            if (string.IsNullOrWhiteSpace(_user.Email) || !IsValidEmail(_user.Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please enter a valid email address.", "OK");
                return;
            }

            // Validate Username
            if (string.IsNullOrWhiteSpace(_user.Username))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please enter a username.", "OK");
                return;
            }

            // Validate Password Length
            if (_user.Password.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Password must be at least 6 characters long.", "OK");
                return;
            }

            try
            {
                var result = await _authClient.CreateUserWithEmailAndPasswordAsync(_user.Email, _user.Password, _user.Username);
                if (!string.IsNullOrWhiteSpace(result?.User?.Info?.Email))
                {
                    await Shell.Current.GoToAsync("//LoginPage");

                }
            }
            catch (FirebaseAuthException ex)
            {
                // Handle Firebase errors (e.g., email already in use)
                var errorMessage = ex.Message;

                if (errorMessage.Contains("EMAIL_EXISTS"))
                {
                    await App.Current.MainPage.DisplayAlert("Registration Failed", "This email address is already in use.", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Registration Failed", "An error occurred. Please try again.", "OK");
                }
            }
        }

        [RelayCommand]
        private async Task NavigateLogin()
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}", true);
        }

        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

}
