using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using MoneyContribution.Models;
using MoneyContribution.Views;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyContribution.ViewModels
{
    public partial class LoginVM : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        /* [ObservableProperty]
         private string _email;
         [ObservableProperty]
         private string _password;
         [ObservableProperty]
         private string _username;*/
        [ObservableProperty]
        private UserModel _user = new();

        public LoginVM(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(_user.Email) || string.IsNullOrWhiteSpace(_user.Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            if (_user.Password.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Password must be at least 6 characters long.", "OK");
                return;
            }

            try
            {
                var result = await _authClient.SignInWithEmailAndPasswordAsync(_user.Email,_user.Password);
                
                if (!string.IsNullOrWhiteSpace(result?.User?.Info?.Email))
                {
                    await FetchUserDetails();
                    AppShell.Current.FlyoutHeader = new FlyoutHeaderControl(_authClient);
                    await Shell.Current.GoToAsync("//DashboardPage");
                }                
            }
            catch (FirebaseAuthException ex)
            {
                var errorMessage = ex.Message;

                if (errorMessage.Contains("Undefined"))
                {
                    await App.Current.MainPage.DisplayAlert("Login Failed", "Check the Internet Connection!", "OK");
                }
                else if (errorMessage.Contains("INVALID_LOGIN_CREDENTIALS"))
                {
                    await App.Current.MainPage.DisplayAlert("Login Failed", "The password is incorrect.", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Login Failed", "An unknown error occurred. Please try again.", "OK");
                }
            }
        }
        private async Task FetchUserDetails()
        {
            try
            {
                var user = _authClient.User;

                if (user != null)
                {
                    _user.Username = !string.IsNullOrEmpty(user.Info.DisplayName) ? user.Info.DisplayName : user.Info.Email;
                }
                else
                {
                    _user.Username = "Anonymous";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user info: {ex.Message}");
                _user.Username = "Anonymous"; 
            }
        }

        [RelayCommand]
        private async Task NavigateRegister()
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }
    }
}
