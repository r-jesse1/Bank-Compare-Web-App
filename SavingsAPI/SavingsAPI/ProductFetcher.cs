namespace SavingsAPI
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SavingsAPI.Models;

    public class ProductFetcher
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<List<Product>> GetSavingsProductsFromApisAsync(List<string> urls)
        {
            var matchingProducts = new List<Product>();

            foreach (var url in urls)
            {
                try
                {
                    string productURL = url + "/banking/products"; 
                    var request = new HttpRequestMessage(HttpMethod.Get, productURL);
                    request.Headers.Add("x-v", "3");
                    Console.WriteLine(productURL);
                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var root = JsonSerializer.Deserialize<Root>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    List<Product> filtered = new();

                    if (root?.Data?.Products != null)
                    {
                        filtered = root.Data.Products
                            .Where(p => p.ProductCategory == "TRANS_AND_SAVINGS_ACCOUNTS")
                            .ToList();

                        // Prefix the productId with the API path
                        foreach (var product in filtered)
                        {
                            product.ProductId = productURL + "/" + product.ProductId;
                        }

                        matchingProducts.AddRange(filtered);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching from {url + "/banking/products"}: {ex.Message}");
                }
            }

            return matchingProducts;
        }
    }
}
