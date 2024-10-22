using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PublicAPI.Services
{
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    namespace PublicAPI.Services
    {
        public class APIFetch
        {
            private readonly HttpClient _httpClient;

            public APIFetch()
            {
                _httpClient = new HttpClient();
            }

            public async Task<T?> GetResultAsync<T>(string url)
            {
                try
                {
                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var data = await response.Content.ReadAsStringAsync();

                    // Handle null cases during deserialization
                    return JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch (HttpRequestException ex)
                {
                    // Log or handle request-related errors
                    Console.WriteLine($"Request error: {ex.Message}");
                    return default;
                }
                catch (JsonException ex)
                {
                    // Log or handle JSON deserialization errors
                    Console.WriteLine($"Serialization error: {ex.Message}");
                    return default;
                }
                catch (Exception ex)
                {
                    // Catch any other unexpected errors
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return default;
                }
            }
        }
    }

}
