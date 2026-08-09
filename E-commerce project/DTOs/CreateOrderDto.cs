namespace E_commerce_project.DTOs
{
    public class CreateOrderDto
    {
        public string userName { get; set; } = string.Empty;
        public List<OrderItemDto> orderItems { get; set; } = new List<OrderItemDto>();
    }
    public class OrderItemDto
    {
        public int productId { get; set; }
        public int quantity { get; set; }
    }
}
