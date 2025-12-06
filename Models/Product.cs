using System.ComponentModel.DataAnnotations;

namespace GymProjectApp.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}