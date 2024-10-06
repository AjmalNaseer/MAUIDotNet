using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Database.Query;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using MoneyContribution.Services;
using MoneyContribution.Models;

namespace MoneyContribution.ViewModels
{
    public partial class DashboardVM : ObservableObject
    {
        private readonly FirebaseClient _firebaseClient;
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private ObservableCollection<Contributions> _contributions = new();

        [ObservableProperty]
        private ObservableCollection<Contributions> _userContributions = new();

        [ObservableProperty]
        private double _collectedMoney;

        [ObservableProperty]
        private double _userContributedMoney;
        private string _currentUserName = "Anonymous";

        public DashboardVM()
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
            LoadUserContributions();
            LoadCollectedMoney();
        }
        public async void RefreshData()
        {
            LoadCollectedMoney();
            LoadUserContributions();
        }
        private async void LoadCollectedMoney()
        {
            try
            {
                var contributions = await _firebaseClient
                    .Child("contributions")
                    .OnceAsync<Contributions>();

                if (contributions != null && contributions.Any())
                {
                   CollectedMoney = contributions.Sum(c => c.Object.Amount);
                }
                else
                {
                    CollectedMoney = 0;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void LoadUserContributions()
        {
            try
            {
                _currentUserName = await FetchUserDetails();
                var contributions = await _firebaseClient
                    .Child("contributions")
                    .OrderBy("Timestamp")
                    .OnceAsync<Contributions>();

                if (contributions != null && contributions.Any())
                {
                    UserContributedMoney = contributions
                        .Where(c => c.Object.UserName == _currentUserName)
                        .Sum(c => c.Object.Amount);

                    var userContributions = contributions
                   .Where(c => c.Object.UserName == _currentUserName)
                   .OrderByDescending(c => DateTime.Parse(c.Object.Timestamp))
                   .Select(c => new Contributions
                   {
                       UserName = c.Object.UserName,
                       Amount = c.Object.Amount,
                       Timestamp = DateTime.Parse(c.Object.Timestamp).ToString("hh:mm tt"),
                       ContributionDate = DateTime.Parse(c.Object.ContributionDate).ToString("dd-MMM-yyyy")
                   })
                   .ToList();

                    UserContributions.Clear();
                    foreach (var contribution in userContributions)
                    {
                        UserContributions.Add(contribution);
                    }
                }
                else
                {
                    UserContributedMoney = 0;
                }                
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Error loading contributions: {ex.Message}", "OK");
            }
        }
        private async Task<string> FetchUserDetails()
        {
            try
            {
                var user = _authClient.User;

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
        private async Task NavigateContribute()
        {
            await Shell.Current.GoToAsync("//ContributePage");
        }

        [RelayCommand]
        private async Task NavigateHistory()
        {
            await Shell.Current.GoToAsync("//HistoryPage");
        }
    }
    
}
