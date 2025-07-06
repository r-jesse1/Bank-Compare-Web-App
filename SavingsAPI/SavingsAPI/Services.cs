using System.Text.Json;
using System.Text.Json.Serialization;
using SavingsAPI.Models;

namespace SavingsAPI.Services
{
    public class SavingsRateExtractor
    {
        private class DepositRate
        {
            [JsonPropertyName("depositRateType")]
            public string Type { get; set; }

            [JsonPropertyName("rate")]
            public string Rate { get; set; }

            [JsonPropertyName("additionalInfo")]
            public string? AdditionalInfo { get; set; }
        }

        private class ProductData
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("brandName")]
            public string BrandName { get; set; }

            [JsonPropertyName("brand")]
            public string Brand { get; set; }

            [JsonPropertyName("productId")]
            public string ProductId { get; set; }

            [JsonPropertyName("depositRates")]
            public List<DepositRate>? DepositRates { get; set; }
        }

        private class BankProduct
        {
            [JsonPropertyName("data")]
            public ProductData Data { get; set; }
        }

        public static SavingsAcc? ParseSavingsAccount(string json, string URL, int id = 0)
        {
            var product = JsonSerializer.Deserialize<BankProduct>(json);

            if (product?.Data?.DepositRates == null) return null;

            var baseRateEntry = product.Data.DepositRates
                .FirstOrDefault(r => r.Type.Equals("VARIABLE", StringComparison.OrdinalIgnoreCase)
                                  || r.Type.Equals("BASE", StringComparison.OrdinalIgnoreCase));

            var bonusRateEntry = product.Data.DepositRates
                .FirstOrDefault(r => r.Type.Equals("INTRODUCTORY", StringComparison.OrdinalIgnoreCase)
                                  || r.Type.Equals("BONUS", StringComparison.OrdinalIgnoreCase));

            float.TryParse(baseRateEntry?.Rate, out float baseRate);
            float.TryParse(bonusRateEntry?.Rate, out float bonusRate);

            string bank = product.Data.BrandName ?? product.Data.Brand;

            Console.WriteLine(bonusRateEntry);


            return new SavingsAcc
            {
                BankProductId = product.Data.ProductId,
                Name = product.Data.Name,
                Bank = bank,
                BaseRate = baseRate,
                BonusRate = bonusRate,
                TotalRate = baseRate + bonusRate,
                BonusConditions = bonusRateEntry?.AdditionalInfo,
                URL = URL,
                Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            };
        }
    }
}