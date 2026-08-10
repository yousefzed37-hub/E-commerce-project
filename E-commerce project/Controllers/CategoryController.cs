using E_commerce_project.DataContext;
using Microsoft.AspNetCore.Mvc;
using E_commerce_project.Models;
using Microsoft.EntityFrameworkCore;
using E_commerce_project.DTOs;

namespace E_commerce_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        private readonly AppDbContext _context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = category.categoryId }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , CategoryDto dto)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if(existingCategory == null)
            {
                return NotFound();
            }
            existingCategory.categoryName = dto.categoryName;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
