namespace E_commerce_project.DTOs
{
    public class ProductFilterDto
    {
        public string? searchTerm { get; set; } 
        public int? categoryId { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
        public string? sortBy { get; set; } 
    }
}
