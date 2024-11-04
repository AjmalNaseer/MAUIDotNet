using PublicAPI.ViewModels;

namespace PublicAPI.Views;

public partial class TicketsPage : ContentPage
{
    public TicketsVM _viewModel;
    public double flexHeight;
    public double flexWidth;
    public TicketsPage()
	{
		InitializeComponent();
        _viewModel = new TicketsVM();
        BindingContext = _viewModel;


    }
    private void RefreshTicketStatuses()
    {
        foreach (var ticket in _viewModel.PagedTickets)
        {
            ticket.IsTimeExceeded = ticket.OrderDateTime < _viewModel.CurrentDateTime && !ticket.IsCompleted;
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
            flexHeight = height * 0.85;
            FlexContainer.HeightRequest = flexHeight;
            flexWidth = width;
            _viewModel.SetScreenWidth(flexHeight, flexHeight);
        
    }



    /*protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        flexHeight = height * 0.95;
        FlexSkillContainer.HeightRequest = flexHeight;
        flexWidth = width;
        
        _viewModel.SetScreenWidth(flexHeight, flexHeight);

    }*/

    /*private void OnContentViewSizeChanged(object sender, EventArgs e)
    {
        if (sender is ContentView contentView)
        {
            double newHeight = contentView.Height;

        }
    }*/
}