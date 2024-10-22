using WebScrapping.ViewModels;
namespace WebScrapping;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPage();

    }
}
