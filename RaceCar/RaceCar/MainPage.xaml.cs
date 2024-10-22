using HtmlAgilityPack;
using OfficeOpenXml;
using RaceCar.Services;
using System.Collections.ObjectModel;

namespace RaceCar
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<ExcelRow> ExcelData { get; set; } = new ObservableCollection<ExcelRow>();

        public MainPage()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set license context here
            string loginUrl = "https://partners.gobx.com//login"; // Your login URL
            webView.Source = loginUrl; // Load the login page in the WebView
            ExcelDataCollectionView.ItemsSource = ExcelData; // Ensure this matches the CollectionView in XAML

        }

        // Load Excel data from a file
        private async void OnLoadExcelClicked(object sender, EventArgs e)
        {
            try
            {
                var filePath = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".xlsx" } },
                        { DevicePlatform.Android, new[] { ".xlsx" } },
                        { DevicePlatform.iOS, new[] { ".xlsx" } }
                    }),
                    PickerTitle = "Select an Excel file"
                });

                if (filePath != null)
                {
                    using (var stream = await filePath.OpenReadAsync())
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet
                            int rowCount = worksheet.Dimension.Rows;

                            ExcelData.Clear(); // Clear previous data

                            for (int row = 2; row <= rowCount; row++) // Assuming first row is header
                            {
                                var excelRow = new ExcelRow
                                {
                                    Column1 = worksheet.Cells[row, 1].Text,
                                    Column2 = worksheet.Cells[row, 2].Text,
                                    Column3 = worksheet.Cells[row, 3].Text,
                                    Column4 = worksheet.Cells[row, 4].Text,
                                    // Add more columns as needed
                                };
                                ExcelData.Add(excelRow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during file loading
                await DisplayAlert("Error", "Failed to load Excel file: " + ex.Message, "OK");
            }
        }

        // Handle scraping operation
        private void OnScrapeButtonClicked(object sender, EventArgs e)
        {
            // Load the Order Booking page in the WebView
            webView.Source = "https://partners.gobx.com//OrderManagement/OrderBooking/OrderBooking";
            webView.Navigated += WebView_Navigated;
        }

        private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            // Log the current URL for debugging
            Console.WriteLine($"Navigated to: {e.Url}");

            // Ensure we are on the Order Booking page
            if (e.Url.Contains("OrderBooking"))
            {
                // Wait a bit for dynamic content to load (adjust timing if necessary)
                await Task.Delay(5000);

                // Extract Customer ID, Email, and Mobile Number directly via JavaScript
                string extractJsScript = @"
            var customerId = document.getElementById('searchCustID') ? document.getElementById('searchCustID').innerText : 'Customer ID not found';
            var email = document.getElementById('searchCustEmail') ? document.getElementById('searchCustEmail').innerText : 'Email not found';
            var mobile = document.getElementById('searchCustPh') ? document.getElementById('searchCustPh').innerText : 'Mobile number not found';
            return customerId + '|' + email + '|' + mobile;
        ";

                // Execute the JavaScript to get the result
                var result = await webView.EvaluateJavaScriptAsync(extractJsScript);

                // Split the result and display
                if (result != null)
                {
                    var details = result.ToString().Split('|');
                    string customerId = details[0];
                    string email = details[1];
                    string mobile = details[2];

                    resultLabel.Text = $"Customer ID: {customerId}\nEmail: {email}\nMobile: {mobile}";

                    // Save results to a file
                    string output = $"Customer ID: {customerId}\nEmail: {email}\nMobile: {mobile}";
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SearchResults.txt");
                    await File.WriteAllTextAsync(path, output);
                }
                else
                {
                    Console.WriteLine("JavaScript execution returned null");
                }
            }
        }


        // Method to parse HTML content and extract Customer ID, Email, and Mobile Number
        private void ParseHtml(string htmlContent, out string customerId, out string email, out string mobile)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // Extract Customer ID
            var customerIdNode = htmlDoc.DocumentNode.SelectSingleNode("//p[@id='searchCustID']");
            customerId = customerIdNode != null ? customerIdNode.InnerText.Trim() : "Customer ID not found";

            // Extract Email
            var emailNode = htmlDoc.DocumentNode.SelectSingleNode("//p[@id='searchCustEmail']");
            email = emailNode != null ? emailNode.InnerText.Trim() : "Email not found";

            // Extract Mobile Number
            var mobileNode = htmlDoc.DocumentNode.SelectSingleNode("//p[@id='searchCustPh']");
            mobile = mobileNode != null ? mobileNode.InnerText.Trim() : "Mobile number not found";
        }


    }
    public class ExcelRow
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        // Add more properties as needed
    }
}
