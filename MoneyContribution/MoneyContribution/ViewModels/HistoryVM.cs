using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Database.Query;
using MoneyContribution.Models;
using MoneyContribution.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyContribution.ViewModels
{
    public partial class HistoryVM : ObservableObject
    {
        private readonly FirebaseClient _firebaseClient;
        private FirebaseAuthClient _authClient;
        
        [ObservableProperty]
        private bool _isBusy;

        private string _currentUserName;

        [ObservableProperty]
        private ObservableCollection<Contributions> _userContributions = new();
        public HistoryVM()
        {
            _firebaseClient = new FirebaseClient(ContribAPIs.FirebaseUrl);
        }
        public async Task InitializeAsync()
        {
            _authClient = FirebaseAuthServices.AuthClient;
            _currentUserName = await FetchUserDetails();
            LoadUserContributions();
        }
        
        private async void LoadUserContributions()
        {
            try
            {
                IsBusy = true;
                var contributions = await _firebaseClient
                    .Child("contributions")
                    .OrderBy("Timestamp") // Order by Timestamp in Firebase
                    .OnceAsync<Contributions>();

                // Filter contributions by current user and reverse to show latest first
                var userContributions = contributions
                    .Where(c => c.Object.UserName == _currentUserName)
                    .OrderByDescending(c => DateTime.Parse(c.Object.Timestamp)) // Sort by original Timestamp
                    .Select(c => new Contributions
                    {
                        UserName = c.Object.UserName,
                        Amount = c.Object.Amount, // Assuming Amount is part of Contribution
                        Timestamp = DateTime.Parse(c.Object.Timestamp).ToString("hh:mm tt"),
                        ContributionDate = DateTime.Parse(c.Object.ContributionDate).ToString("dd-MMM-yyyy")
                    })
                    .ToList();

                // Clear existing contributions
                UserContributions.Clear();

                // Add filtered and formatted contributions
                foreach (var contribution in userContributions)
                {
                    UserContributions.Add(contribution);
                }
                IsBusy = false;
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
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("//DashboardPage");
        }

    }
}
