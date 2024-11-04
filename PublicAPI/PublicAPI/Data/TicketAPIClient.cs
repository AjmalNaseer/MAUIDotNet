using PublicAPI.Models;
using PublicAPI.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PublicAPI.Data
{
    public class TicketAPIClient
    {
        private readonly HttpClient _httpClient;
        private readonly EndPoints _endPoints;

        public TicketAPIClient()
        {
            _httpClient = new HttpClient();
            _endPoints = new EndPoints();
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_endPoints.GetAllTickets);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Ticket>>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return new List<Ticket>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Serialization error: {ex.Message}");
                return new List<Ticket>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Ticket>();
            }
        }

        public async Task<Ticket> GetTicketByNumberAsync(string ticketNumber)
        {
            try
            {
                var response = await _httpClient.GetAsync(_endPoints.GetTicketByNumber(ticketNumber));
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Ticket>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Serialization error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Ticket>> GetPendingTicketsAsync()
        {
            var allTickets = await GetAllTicketsAsync();
            return allTickets.FindAll(t => !t.IsCompleted);
        }

        public async Task CreateTicketAsync(Ticket ticket)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_endPoints.GetAllTickets, ticket);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task UpdateTicketAsync(string ticketNumber, Ticket ticket)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(_endPoints.GetTicketByNumber(ticketNumber), ticket);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task DeleteTicketAsync(string ticketNumber)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(_endPoints.GetTicketByNumber(ticketNumber));
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
