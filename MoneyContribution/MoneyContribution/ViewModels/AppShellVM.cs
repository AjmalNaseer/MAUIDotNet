using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using MoneyContribution.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyContribution.ViewModels
{
    public partial class AppShellVM : BaseViewModel
    {
        private readonly FirebaseAuthClient _firebaseAuthClient;
        public AppShellVM(FirebaseAuthClient firebaseAuthClient)
        {
            _firebaseAuthClient = firebaseAuthClient;
        }
        /*[RelayCommand]
        async void NavigateDashboard()
        {
            await Shell.Current.GoToAsync("//DashboardPage");

        }
        [RelayCommand]
        async void NavigateContribute()
        {
            await Shell.Current.GoToAsync("//ContributePage");

        }
        [RelayCommand]
        async void NavigateHistory()
        {
            await Shell.Current.GoToAsync("//HistoryPage");

        }*/
        [RelayCommand]
        async void Logout()
        {
            bool result = await Shell.Current.DisplayAlert("Logout", "Are you Sure you want to Logout?", "Yes", "No");
            if (result)
            {
                _firebaseAuthClient.SignOut();
                
                await Shell.Current.GoToAsync("//LoginPage");

            }
        }
    }
}
