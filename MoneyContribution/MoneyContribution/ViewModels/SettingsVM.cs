using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyContribution.ViewModels
{
    public partial class SettingsVM : ObservableObject
    {
        [RelayCommand]
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("//DashboardPage");
        }
    }
}
