namespace E_commerce_project.DTOs
{
    public class CreateProductDto
    {
        public string productName { get; set; } = string.Empty;
        public string productDescription { get; set; } = string.Empty;
        public decimal price { get; set; }
        public int stockQuantity { get; set; }
        public IFormFile? image { get; set; } 
        public int categoryId { get; set; }
    }
}
