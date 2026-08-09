using System.ComponentModel.DataAnnotations;

namespace E_commerce_project.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
    public class Order
    {
        [Key]
        public int orderId { get; set; }
        [Required]
        public string userName { get; set; } = string.Empty;
        public DateTime orderDate { get; set; } = DateTime.Now;
        public decimal totalAmount { get; set; }
        public ICollection<OrderItem> orderItems { get; set; } = new List<OrderItem>();
        public OrderStatus orderStatus { get; set; } = OrderStatus.Pending;
    }
}
