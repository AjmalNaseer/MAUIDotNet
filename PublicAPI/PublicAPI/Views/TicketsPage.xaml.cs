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
   
}