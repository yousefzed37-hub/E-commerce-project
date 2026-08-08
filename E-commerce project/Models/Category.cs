using System.ComponentModel.DataAnnotations;

namespace E_commerce_project.Models
{
    public class Category
    {
        [Key]
        public int categoryId { get; set; }
        public string categoryDescription { get; set; } = string.Empty;
        [Required]
        public string categoryName { get; set; } = string.Empty;
        //1 to many relationship with Product
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
