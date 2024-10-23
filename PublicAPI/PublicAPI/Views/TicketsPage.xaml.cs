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
        SetColumnSpan();

    }
    private void RefreshTicketStatuses()
    {
        // Call this method to update IsTimeExceeded status
        foreach (var ticket in _viewModel.Tickets)
        {
            ticket.IsTimeExceeded = ticket.OrderDateTime < _viewModel.CurrentDateTime && !ticket.IsCompleted;
        }
    }
    private void SetColumnSpan()
    {
        double screenWidth = DeviceDisplay.MainDisplayInfo.Width; // Get the screen width in pixels
        double ticketWidth = 168; // Width of each ticket
        double totalWidth = 1296; // The total width you want to allocate

        // Calculate number of columns that fit within the screen width
        int numberOfColumns = (int)(totalWidth / ticketWidth);

        // Set the span dynamically
        var gridLayout = (GridItemsLayout)CollectionViewTickets.ItemsLayout;
        gridLayout.Span = numberOfColumns;
    }
  
}