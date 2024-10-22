using WebScrapping.ViewModels;

namespace WebScrapping.Views;
public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = new MainPage();

    }
}