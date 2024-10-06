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
    public partial class LoadingPageVM : BaseViewModel
    {
        private readonly FirebaseAuthClient _firebaseAuthClient;
        public LoadingPageVM(FirebaseAuthClient firebaseAuthClient) 
        {
            _firebaseAuthClient = firebaseAuthClient;
            CheckUserLoginDetails();
        }

        private async void CheckUserLoginDetails()
        {
            if (!string.IsNullOrWhiteSpace(_firebaseAuthClient?.User?.Info?.Email))
            {
                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    AppShell.Current.Dispatcher.Dispatch(async () =>
                    {
                        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");

                    });
                }
                else 
                {
                    await Shell.Current.GoToAsync($"{nameof(LoginPage)}");

                }
            }
        }
    }
}
