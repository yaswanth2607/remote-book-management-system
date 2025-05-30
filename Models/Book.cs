using System.ComponentModel.DataAnnotations;

namespace SecondHandBookEkartApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author {  get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; } 

        [Required]
        public string Location { get; set; }

        [Required]
        public string SellerName {  get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid Indian mobile number")]
        public string MobileNumber {  get; set; }

        [Required]
        public int StockAvailable { get; set; }

        [Required]
        public string Deletekey { get; set; }
    }
}
