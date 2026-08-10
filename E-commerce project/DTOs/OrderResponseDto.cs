namespace E_commerce_project.DTOs
{
    public class OrderResponseDto
    {
        public int orderId { get; set; }
        public string userName { get; set; } = string.Empty;
        public DateTime orderDate { get; set; }
        public decimal totalAmount { get; set; }
        public string orderStatus { get; set; } = string.Empty; // إرجاع الحالة كـ string بدلاً من enum number
        public List<OrderItemResponseDto> orderItems { get; set; } = new();
    }
}
