using Microsoft.AspNetCore.Http.HttpResults;
using SavingsAPI.Models;
using SavingsAPI.URLs;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            [JsonPropertyName("additionalInfoUri")]
            public string? AdditionalInfoUri { get; set; }
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
            
            [JsonPropertyName("applicationUri")]
            public string ApplicationUri { get; set; }

            [JsonPropertyName("additionalInformation")]
            public AdditionalInformation AdditionalInformation { get; set; }
        }

        private class BankProduct
        {
            [JsonPropertyName("data")]
            public ProductData Data { get; set; }
        }
        
        private class AdditionalInformation
        {
            [JsonPropertyName("overviewUri")]
            public string OverviewUri { get; set; }

            [JsonPropertyName("feesAndPricingUri")]
            public string FeesAndPricingUri { get; set; }

        }
        public static SavingsAcc? ParseSavingsAccount(string json, string URL, int id = 0)
        {
            var product = JsonSerializer.Deserialize<BankProduct>(json);

            if (product?.Data?.DepositRates == null) return null;

            var baseRateEntry = product.Data.DepositRates
                .FirstOrDefault(r => r.Type.Equals("VARIABLE", StringComparison.OrdinalIgnoreCase)
                                  || r.Type.Equals("BASE", StringComparison.OrdinalIgnoreCase));

            var bonusRateEntry = product.Data.DepositRates
                .FirstOrDefault(r => r.Type.Equals("BONUS", StringComparison.OrdinalIgnoreCase));

            var introRateEntry = product.Data.DepositRates
                .FirstOrDefault(r => r.Type.Equals("INTRODUCTORY", StringComparison.OrdinalIgnoreCase));

            float.TryParse(baseRateEntry?.Rate, out float baseRate);
            float.TryParse(bonusRateEntry?.Rate, out float bonusRate);
            float.TryParse(introRateEntry?.Rate, out float introRate);

            var uri = product.Data.ApplicationUri
                ?? product.Data.AdditionalInformation?.OverviewUri
                ?? baseRateEntry?.AdditionalInfoUri
                ?? product.Data.AdditionalInformation?.FeesAndPricingUri
                ?? string.Empty;

            string bank = product.Data.BrandName ?? product.Data.Brand;
            if (bank == "") bank = "Abal Banking";
            if (bank == "Lending and bank accounts, both associated with BSB number 939 200") bank = "AMP";

            string cleanName = product.Data.Name.Replace(bank, "").Trim();

            // TCU already has their rates in percentages
            if (!string.Equals(bank, "TCU", StringComparison.OrdinalIgnoreCase))
            {
                baseRate = (float)Math.Round(baseRate * 100, 2);
                bonusRate = (float)Math.Round(bonusRate * 100, 2);
                introRate = (float)Math.Round(introRate * 100, 2);
            }

            // DBL already has the base rate included in the bonus rate
            if (string.Equals(bank, "Defence Bank Limited", StringComparison.OrdinalIgnoreCase))
            {
                bonusRate -= baseRate;
            }


            float totalRate;

            if (introRate > 0 && bonusRate > 0)
            {
                totalRate = baseRate + introRate + bonusRate;
            }
            else if (introRate > 0)
            {
                totalRate = introRate;
            }
            else
            {
                totalRate = baseRate + bonusRate;
            }

            
            if (string.Equals(bank, "ubank", StringComparison.OrdinalIgnoreCase))
            {
                totalRate -= baseRate;
            }



            return new SavingsAcc
            {
                BankProductId = product.Data.ProductId,
                Name = cleanName,
                Bank = bank,
                BaseRate = baseRate,
                BonusRate = bonusRate,
                IntroRate = introRate,
                TotalRate = totalRate,
                BonusConditions = bonusRateEntry?.AdditionalInfo,
                IntroConditions = introRateEntry?.AdditionalInfo,
                URL = URL,
                ProductURL = uri,
                Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) 
            };
        }
    }
}