using PublicAPI.ViewModels;

namespace PublicAPI.Views;

public partial class TicketsPage : ContentPage
{
    public TicketsVM _viewModel;
    public TicketsPage()
	{
		InitializeComponent();
        _viewModel = new TicketsVM();
        BindingContext = _viewModel;


    }
    private void RefreshTicketStatuses()
    {
        foreach (var ticket in _viewModel.Tickets)
        {
            ticket.IsTimeExceeded = ticket.OrderDateTime < _viewModel.CurrentDateTime && !ticket.IsCompleted;
        }
    }
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        _viewModel.SetScreenWidth(width);
        double requiredWidth = (168 * 7) + (15 * 6);
        FlexSkillContainer.WidthRequest = Math.Min(width, requiredWidth);
        double flexHeight = height * 0.95;
        FlexSkillContainer.HeightRequest = flexHeight;
    }
   
}