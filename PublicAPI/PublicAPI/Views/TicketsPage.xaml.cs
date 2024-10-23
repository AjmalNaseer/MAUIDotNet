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
        PopulateTickets();
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
    private void PopulateTickets()
    {
        int columnCount = 7; // Assuming each ticket has a fixed width of 200pt
        int currentColumn = 0; // Track the current column index
        double totalHeight = 0;
        double maxHeight = 660; // Fixed height for the CollectionView

        foreach (var ticket in _viewModel.Tickets)
        {
            // Create a new stack for each ticket
            var ticketLayout = new StackLayout
            {
                WidthRequest = 168,
                VerticalOptions = LayoutOptions.StartAndExpand,
                // Populate with ticket content
            };

            // Measure the height of the ticketLayout
            ticketLayout.Measure(168, double.PositiveInfinity);

            double ticketHeight = ticketLayout.Height; // Use the measured height
            totalHeight += ticketHeight; // Update total height

            // Check if the total height exceeds the fixed height for the CollectionView
            if (totalHeight > maxHeight)
            {
                currentColumn++; // Move to the next column
                totalHeight = ticketHeight; // Start new column with current ticket height

                // Check if we've reached max columns
                if (currentColumn >= columnCount)
                {
                    break; // Stop if we've filled all columns
                }
            }

            // Set the ticket layout in the grid
            MainGrid.Children.Add(ticketLayout);
            Grid.SetColumn(ticketLayout, currentColumn);
            Grid.SetRow(ticketLayout, 0); // This will change in case of a next column
        }
    }




}