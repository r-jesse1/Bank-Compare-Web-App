using System.ComponentModel.DataAnnotations;

namespace SavingsAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? ProductId { get; set; }
        public string? ProductCategory { get; set; }
        public string? Name { get; set; }
        public string? BrandName { get; set; }
    }

    public class ProductData
    {
        public List<Product> Products { get; set; }
    }

    public class Root
    {
        public ProductData Data { get; set; }
    }
}
