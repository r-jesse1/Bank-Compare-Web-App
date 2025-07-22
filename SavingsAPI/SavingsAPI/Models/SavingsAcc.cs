using System.ComponentModel.DataAnnotations;

namespace SavingsAPI.Models
{
    public class SavingsAcc
    {
        [Key]
        public int Id { get; set; }
        [Required]
        required public string Name { get; set; }
        [Required]
        required public string Bank { get; set; }
        [Required]
        public required string BankProductId { get; set; }
        public string? BonusConditions { get; set; }
        public string? IntroConditions { get; set; }
        [Required]
        required public float BaseRate { get; set; }
        public float BonusRate { get; set; }
        public float IntroRate { get; set; }

        [Required]
        public float TotalRate { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public required string URL { get; set; }
        public required string ProductURL { get; set; }


    }
}
