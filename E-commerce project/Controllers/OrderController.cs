using E_commerce_project.DataContextContext;
using E_commerce_project.DTOs;
using E_commerce_project.Models;
using Microsoft.AspNetCore.Authorization;
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
            var orders = await _context.Orders.Include(o => o.orderItems).ThenInclude(oi => oi.Product).Select(o => new OrderResponseDto
            {
                orderId = o.orderId,
                userName = o.userName,
                orderDate = o.orderDate,
                totalAmount = o.totalAmount,
                orderStatus = o.orderStatus.ToString(),
                orderItems = o.orderItems.Select(oi => new OrderItemResponseDto
                {
                    id = oi.Id,
                    productId = oi.ProductId,
                    productName = oi.Product.productName,
                    quantity = oi.Quantity,
                    unitPrice = oi.UnitPrice
                }).ToList()
            }).ToListAsync();
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
                totalAmount += orderItem.UnitPrice * itemDto.quantity; // 👈 حساب السعر الإجمالي
                order.orderItems.Add(orderItem);
            }
            order.totalAmount = totalAmount; // 👈 إضافة السعر الإجمالي المحسوب للطلب
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // 👈 بتضيف السطرين دول هنا
            var createdOrder = await _context.Orders
                .Include(o => o.orderItems)
                .FirstOrDefaultAsync(o => o.orderId == order.orderId);

            // 👈 وبتخلي الـ return ترجع createdOrder بدل order
            return CreatedAtAction(nameof(GetById), new { id = order.orderId }, createdOrder);
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
