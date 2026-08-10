namespace E_commerce_project.DTOs
{
    public class OrderItemResponseDto
    {
        public int id { get; set; }
        public int productId { get; set; }
        public string productName { get; set; } = string.Empty;
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }

        //front end
        public decimal itemTotalPrice => unitPrice * quantity;
    }
}
