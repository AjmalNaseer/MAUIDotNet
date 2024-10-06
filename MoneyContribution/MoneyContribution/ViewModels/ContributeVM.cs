using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth.Providers;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MoneyContribution.Services;
using MoneyContribution.Models;

namespace MoneyContribution.ViewModels
{
    public partial class ContributeVM : ObservableObject
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private string _contributionAmount;

        [ObservableProperty]
        private double _collectedMoney;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private ObservableCollection<Contributions> _userContributions = new();

        private string _currentUserName = "Anonymous";

        // Constructor initializes Firebase clients
        public ContributeVM()
        {
            _firebaseClient = new FirebaseClient(ContribAPIs.FirebaseUrl);

            _authClient = new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = ContribAPIs.ApiKey,
                AuthDomain = ContribAPIs.AuthDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider(),
                    new EmailProvider()
                }
            });
        }

        [RelayCommand]
        private async Task NumberEnter(string number)
        {
            if (string.IsNullOrEmpty(ContributionAmount) || ContributionAmount == "0")
            {
                // If ContributionAmount is empty or "0", replace it with the pressed number
                ContributionAmount = number;
            }
            else
            {
                // Otherwise, append the pressed number
                ContributionAmount += number;
            }
        }

        [RelayCommand]
        private async Task Delete()
        {
            if (!string.IsNullOrEmpty(ContributionAmount))
            {
                ContributionAmount = ContributionAmount.Substring(0, ContributionAmount.Length - 1);
            }
        }
        [RelayCommand]
        private async Task ContributeAsync()
        {
            if (double.TryParse(ContributionAmount, out double contribution))
            {
                // Check if the contribution amount is zero or negative
                if (contribution <= 0)
                {
                    // Show an alert if the amount is zero or less
                    await App.Current.MainPage.DisplayAlert("Invalid Amount", "Please enter an amount greater than zero.", "OK");
                    return; // Exit the method
                }

                try
                {
                    _currentUserName = await GetCurrentUserNameAsync();

                    var contributionData = new Contributions
                    {
                        Amount = contribution,
                        Timestamp = DateTime.UtcNow.ToString("o"),
                        UserName = _currentUserName,
                        ContributionDate = DateTime.UtcNow.ToString("yyyy-MM-dd")
                    };

                    await _firebaseClient
                        .Child("contributions")
                        .PostAsync(contributionData);

                    CollectedMoney += contribution;
                    ContributionAmount = string.Empty;
                    UserContributions.Add(contributionData);

                    await App.Current.MainPage.DisplayAlert("Thank You", "Your contribution successfully added.", "OK");
                    await Shell.Current.GoToAsync("//DashboardPage");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", $"Firebase error: {ex.Message}", "OK");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please enter a valid amount.", "OK");
            }
        }




        // Fetch the current user's display name or email from Firebase Authentication
        private async Task<string> GetCurrentUserNameAsync()
        {
            try
            {
                var user = _authClient.User;

                if (user != null)
                {
                    // Return the display name if available, otherwise use the email
                    return !string.IsNullOrEmpty(user.Info.DisplayName) ? user.Info.DisplayName : user.Info.Email;
                }
                else
                {
                    return "Anonymous";
                }
            }
            catch (Exception ex)
            {
                // Log the error and return a default value
                Console.WriteLine($"Error fetching user info: {ex.Message}");
                return "Anonymous";
            }
        }

        [RelayCommand]
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("//DashboardPage");
        }
    }

}
