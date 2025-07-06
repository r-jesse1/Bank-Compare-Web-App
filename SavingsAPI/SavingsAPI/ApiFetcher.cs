namespace SavingsAPI
{
    using Microsoft.EntityFrameworkCore;
    using SavingsAPI.Data;
    using SavingsAPI.Models;
    using SavingsAPI.Services;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ApiFetcher
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<string> GetJsonFromCBAAsync()
        {
            //string url = "https://api.commbank.com.au/public/cds-au/v1/banking/products/ad22b1f0967349e8a5d586afe7f0d845"; 5
            // string url = "https://cdr.apix.anz/cds-au/v1/banking/products/SAVING01"; // 4
            string url = "https://digital-api.westpac.com.au/cds-au/v1/banking/products/SavESaver"; // 4

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-v", "4");


                var response = await httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode(); // throws if not 2xx

                string json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Error fetching JSON: " + ex.Message);
                return null;
            }
        }

        public static async Task FetchAndSaveProductAsync(string url, SavingsDbContext dbContext)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-v", "4");

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                var product = SavingsRateExtractor.ParseSavingsAccount(json, url);

                if (product != null)
                {
                    var existing = await dbContext.SavingsAccounts.FirstOrDefaultAsync(p =>
                        p.URL == product.URL &&
                        p.Date == product.Date
                        
                    );

                    if (existing != null)
                    {
                        // Update existing record
                        existing.Name = product.Name;
                        existing.Bank = product.Bank;
                        existing.BaseRate = product.BaseRate;
                        existing.BonusRate = product.BonusRate;
                        existing.BonusConditions = product.BonusConditions;

                        Console.WriteLine($"Updated product {url} in database.");
                    }
                    else
                    {
                        dbContext.SavingsAccounts.Add(product);
                        Console.WriteLine($"Added new product {url} to database.");
                    }

                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine($"Cannot add null product: {url}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
