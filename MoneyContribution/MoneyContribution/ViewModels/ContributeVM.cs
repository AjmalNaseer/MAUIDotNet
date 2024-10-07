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
        private FirebaseAuthClient _authClient;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _contributionAmount;

        [ObservableProperty]
        private ObservableCollection<Contributions> _userContributions = new();

        private string _currentUserName;

        public ContributeVM()
        {
            _firebaseClient = new FirebaseClient(ContribAPIs.FirebaseUrl);
        }

        public async Task InitializeAsync()
        {
            _authClient = FirebaseAuthServices.AuthClient;
            _currentUserName = await FetchUserDetails();
        }

        [RelayCommand]
        private async Task NumberEnter(string number)
        {
            if (string.IsNullOrEmpty(ContributionAmount) || ContributionAmount == "0")
            {
                ContributionAmount = number;
            }
            else
            {
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
                if (contribution <= 0)
                {
                    await App.Current.MainPage.DisplayAlert("Invalid Amount", "Please enter an amount greater than zero.", "OK");
                    return; 
                }

                try
                {
                    IsBusy = true;
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

                    ContributionAmount = string.Empty;
                    UserContributions.Add(contributionData);
                    IsBusy = false;
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

        private async Task<string> FetchUserDetails()
        {
            try
            {
                var user = FirebaseAuthServices.AuthClient.User;

                if (user != null)
                {
                    return !string.IsNullOrEmpty(user.Info.DisplayName) ? user.Info.DisplayName : user.Info.Email;
                }
                else
                {
                    return "Anonymous";
                }
            }
            catch (Exception ex)
            {
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
