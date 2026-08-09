using E_commerce_project.DataContext;
using E_commerce_project.DTOs;
using E_commerce_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders.Include(o => o.orderItems).ThenInclude(oi => oi.Product).ToListAsync();
            return Ok(orders);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders.Include(o => o.orderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.orderId == id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            if(dto.orderItems == null  || !dto.orderItems.Any())
            {
                return BadRequest("Order must contain at least one item.");
            }

            var order = new Order
            {
                userName = dto.userName,
                orderDate = DateTime.UtcNow,
                orderStatus = OrderStatus.Pending,
                orderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach(var itemDto in dto.orderItems)
            {
                var product = await _context.Products.FindAsync(itemDto.productId);
                if (product == null)
                {
                    return BadRequest($"Product with ID {itemDto.productId} not found.");
                }

                if(product.stockQuantity < itemDto.quantity)
                {
                    return BadRequest($"Insufficient stock for product with ID {itemDto.productId}.");
                }
                product.stockQuantity -= itemDto.quantity;
                var orderItem = new OrderItem
                {
                    ProductId = itemDto.productId,
                    Quantity = itemDto.quantity,
                    UnitPrice = product.price
                };
                totalAmount += orderItem.Quantity * orderItem.UnitPrice;
                order.orderItems.Add(orderItem);
            }
            order.totalAmount = totalAmount; // 👈 إضافة السعر الإجمالي المحسوب للطلب
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = order.orderId }, order);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderStatus updatedOrder)
        {
            var order = await _context.Orders.Include(o => o.orderItems).FirstOrDefaultAsync(o => o.orderId == id);
            if (order == null)
            {
                return NotFound();
            }
            order.orderStatus = updatedOrder;
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
