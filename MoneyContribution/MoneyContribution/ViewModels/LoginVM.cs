using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using MoneyContribution;
using MoneyContribution.Models;
using MoneyContribution.Services;

public partial class LoginVM : ObservableObject
{
    private readonly FirebaseAuthClient _authClient;

    [ObservableProperty]
    private UserModel _user = new();

    public LoginVM()
    {
        _authClient = FirebaseAuthServices.AuthClient;
    }

    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrWhiteSpace(User.Email) || string.IsNullOrWhiteSpace(User.Password))
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
            var result = await FirebaseAuthServices.SignInUserAsync(User.Email, User.Password);

            if (!string.IsNullOrWhiteSpace(result?.User?.Info?.Email))
            {
                await FetchUserDetails();
                AppShell.Current.FlyoutHeader = new FlyoutHeaderControl(_authClient);
                await NavigateDashboard();
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
            var user = FirebaseAuthServices.AuthClient.User;

            if (user != null)
            {
                User.Username = !string.IsNullOrEmpty(user.Info.DisplayName) ? user.Info.DisplayName : user.Info.Email;
            }
            else
            {
                User.Username = "Anonymous";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user info: {ex.Message}");
            User.Username = "Anonymous";
        }
    }

    [RelayCommand]
    private async Task NavigateRegister()
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }

    [RelayCommand]
    private async Task NavigateDashboard()
    {
        await Shell.Current.GoToAsync("//DashboardPage");
    }
}
