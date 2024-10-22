using HtmlAgilityPack;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapping.Models;

namespace WebScrapping.Services
{
    public class DataFetcher
    {
        private static readonly HttpClient client = new HttpClient();

        // URLs
        private static string loginUrl = "https://partners.gobx.com/login";
        private static string searchUrl = "https://partners.gobx.com/OrderManagement/OrderBooking/OrderBooking";

        // Login credentials
        private static string username = "Rt0409412";
        private static string password = "Abcd1234";

        public async Task Main(string[] args)
        {
            // Login
            if (await LoginAsync())
            {
                Console.WriteLine("Login successful");

                // Read customer IDs from Excel
                var filePath = "customer_data.xlsx";
                var package = new ExcelPackage(new FileInfo(filePath));

                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                // Iterate through each customer ID
                for (int row = 2; row <= rowCount; row++)
                {
                    string customerId = worksheet.Cells[row, 1].Text;

                    // Fetch customer details
                    var customerData = await FetchCustomerDataAsync(customerId);

                    // Update Excel sheet with the fetched data
                    if (customerData != null)
                    {
                        worksheet.Cells[row, 2].Value = customerData.CustomerNumber;
                        worksheet.Cells[row, 3].Value = customerData.Email;
                        worksheet.Cells[row, 4].Value = customerData.MobileNumber;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to fetch data for CustomerID: {customerId}");
                    }
                }

                // Save the updated Excel file
                package.SaveAs(new FileInfo("customer_data_updated.xlsx"));
                Console.WriteLine("Customer data updated and saved to Excel.");
            }
            else
            {
                Console.WriteLine("Login failed");
            }
        }

        public async Task<bool> LoginAsync()
        {
            var loginData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("UserName", username),
                new KeyValuePair<string, string>("Password", password)
            });

            var response = await client.PostAsync(loginUrl, loginData);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody.Contains("Logout"); // Modify this based on the actual response after login
            }
            return false;
        }

        public async Task<CustomerData> FetchCustomerDataAsync(string customerId)
        {
            var searchParams = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("SearchNbr", customerId),
                new KeyValuePair<string, string>("Type", "C") // "C" for Customer Number search
            });

            var searchResponse = await client.PostAsync(searchUrl, searchParams);

            if (searchResponse.IsSuccessStatusCode)
            {
                var html = await searchResponse.Content.ReadAsStringAsync();
                return ParseHtmlForCustomerData(html);
            }

            return null;
        }

        private static CustomerData ParseHtmlForCustomerData(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var customerNumberNode = doc.DocumentNode.SelectSingleNode("//p[@id='searchCustID']");
            var emailNode = doc.DocumentNode.SelectSingleNode("//p[@id='searchCustEmail']");
            var mobileNumberNode = doc.DocumentNode.SelectSingleNode("//p[@id='searchCustPh']");

            if (customerNumberNode != null && emailNode != null && mobileNumberNode != null)
            {
                return new CustomerData
                {
                    CustomerNumber = customerNumberNode.InnerText.Trim(),
                    Email = emailNode.InnerText.Trim(),
                    MobileNumber = mobileNumberNode.InnerText.Trim()
                };
            }

            return null;
        }
    }

    
}
