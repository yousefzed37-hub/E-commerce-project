using E_commerce_project.DataContext;
using E_commerce_project.DTOs;
using E_commerce_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.Include(p => p.category).ToListAsync();  
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> FindById(int id)
        {
             var product = await _context.Products.Include(p => p.category).FirstOrDefaultAsync(p => p.productId == id);
             if(product == null)
             {
                return NotFound();
             }
             return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            // 1. التأكد إن الـ CategoryId موجود فعلاً في الداتابيز
            var categoryExists = await _context.Categories.AnyAsync(c => c.categoryId == dto.categoryId);
            if (!categoryExists)
            {
                return BadRequest(new { message = $"Category with ID {dto.categoryId} does not exist." });
            }

            // 2. تحويل الـ DTO لـ Product Model
            var product = new Product
            {
                productName = dto.productName,
                productDescription = dto.productDescription,
                price = dto.price,
                stockQuantity = dto.stockQuantity,
                imageUrl = dto.imageUrl,
                categoryId = dto.categoryId,
                createdDate = DateTime.UtcNow
            };

            // 3. الحفظ في الداتابيز
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(FindById), new { id = product.productId }, product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            // 1. البحث عن المنتج في الداتابيز
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }

            // 2. التأكد إن الـ Category الجديدة موجودة فعلاً
            var categoryExists = await _context.Categories.AnyAsync(c => c.categoryId == dto.categoryId);
            if (!categoryExists)
            {
                return BadRequest(new { message = $"Category with ID {dto.categoryId} does not exist." });
            }

            // 3. تحديث البيانات
            existingProduct.productName = dto.productName;
            existingProduct.productDescription = dto.productDescription;
            existingProduct.price = dto.price;
            existingProduct.stockQuantity = dto.stockQuantity;
            existingProduct.imageUrl = dto.imageUrl;
            existingProduct.categoryId = dto.categoryId;

            // 4. حفظ التعديلات
            await _context.SaveChangesAsync();

            return NoContent(); // أو return Ok(existingProduct);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found." });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfully." });
        }
    }
}
