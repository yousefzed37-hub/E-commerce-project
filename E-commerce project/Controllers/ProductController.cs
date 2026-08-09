using E_commerce_project.DataContext;
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
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            var CategoryExists = await _context.Categories.AnyAsync(c => c.categoryId == product.categoryId);
            if(!CategoryExists)
            {
                return BadRequest();
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(FindById), new { id = product.productId }, product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , [FromBody] Product UpdatedProduct)
        {
            if (id != UpdatedProduct.productId)
            {
                return BadRequest();
            }
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            existing.productName = UpdatedProduct.productName;
            existing.productDescription = UpdatedProduct.productDescription;
            existing.price = UpdatedProduct.price;
            existing.stockQuantity = UpdatedProduct.stockQuantity;
            existing.imageUrl = UpdatedProduct.imageUrl;
            existing.categoryId = UpdatedProduct.categoryId;

            await _context.SaveChangesAsync();

            return NoContent();
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
