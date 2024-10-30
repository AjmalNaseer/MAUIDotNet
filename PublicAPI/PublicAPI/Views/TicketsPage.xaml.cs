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
        // Call this method to update IsTimeExceeded status
        foreach (var ticket in _viewModel.Tickets)
        {
            ticket.IsTimeExceeded = ticket.OrderDateTime < _viewModel.CurrentDateTime && !ticket.IsCompleted;
        }
    }
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        // Calculate the required width for 7 columns with 15pt spacing
        double requiredWidth = (168 * 7) + (15 * 6);

        // Set the container width request to fit 7 columns with spacing
        FlexSkillContainer.WidthRequest = Math.Min(width, requiredWidth); // Ensures it doesn't exceed screen width

        // Calculate the FlexSkillContainer height based on screen height (e.g., 95% of screen height)
        double flexHeight = height * 0.95;
        FlexSkillContainer.HeightRequest = flexHeight;
    }
   
}