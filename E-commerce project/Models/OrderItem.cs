using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_project.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [ForeignKey("Order")]   
        public int OrderId { get; set; }
        
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ItemTotalPrice => UnitPrice * Quantity;
    }
}
