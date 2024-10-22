using HtmlAgilityPack;
using PuppeteerSharp;
using System.Threading.Tasks;

namespace RaceCar.Services
{
    public class WebScraper
    {
        // Method to scrape a website after login
        public async Task<string> ScrapeWebsiteAsync(string targetUrl)
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe" // Ensure this path is correct
            });

            var page = await browser.NewPageAsync();
            await page.GoToAsync(targetUrl, new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle2 } });

            // Get the content of the page after navigation
            var content = await page.GetContentAsync();
            await browser.CloseAsync();
            return content;
        }

        // Method to parse HTML content and extract necessary data
        public string ParseHtml(string htmlContent)
        {
            if (string.IsNullOrEmpty(htmlContent))
                return "No content to parse.";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // Extract Customer Number, Email, and Mobile Number
            var customerIdNode = htmlDoc.DocumentNode.SelectSingleNode("//p[@id='searchCustID']"); // Update selector as needed
            var emailNode = htmlDoc.DocumentNode.SelectSingleNode("//p[@id='searchCustEmail']"); // Update selector as needed
            var mobileNode = htmlDoc.DocumentNode.SelectSingleNode("//p[@id='searchCustPh']"); // Update selector as needed

            var customerId = customerIdNode?.InnerText.Trim() ?? "Customer ID not found";
            var email = emailNode?.InnerText.Trim() ?? "Email not found";
            var mobile = mobileNode?.InnerText.Trim() ?? "Mobile number not found";

            // Format and return the parsed data
            return $"Customer ID: {customerId}\nEmail: {email}\nMobile: {mobile}";
        }
    }
}
