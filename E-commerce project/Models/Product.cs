using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_project.Models
{
    public class Product
    {
        [Key]
        public int productId { get; set; }
        [Required]
        public string productName { get; set; } = string.Empty;
        public string productDescription { get; set; } = string.Empty;
        [Required]
        public decimal price { get; set; }
        [Required]
        public int stockQuantity { get; set; }
        public string imageUrl { get; set; } = string.Empty;
        public DateTime createdDate { get; set; } = DateTime.Now;
        [ForeignKey("Category")]
        public int categoryId { get; set; }
        public Category category { get; set; } = null!;
    }
}
