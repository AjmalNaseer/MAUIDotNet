using MoneyContribution.ViewModels;
using MoneyContribution.Views;

namespace MoneyContribution
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellVM viewmodel)
        {
            InitializeComponent();
            BindingContext = viewmodel;
        }

    }
}
