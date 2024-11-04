using System;

namespace PublicAPI.Services
{
    public class EndPoints
    {
        public string BaseUrl { get; } = "https://api.coindesk.com/v1/";
        public string GetAllTickets => $"{BaseUrl}api/tickets";
        public string GetTicketByNumber(string ticketNumber) => $"{BaseUrl}api/tickets/{ticketNumber}";
    }
}
