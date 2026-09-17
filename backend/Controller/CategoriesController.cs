using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShopAPI.Data;
using PerfumeShopAPI.DTOs;
using PerfumeShopAPI.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{

    private readonly PerfumeShopeContext _context;

    public CategoriesController(PerfumeShopeContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllCategories()
    {
        var categories = await _context.Categories.ToListAsync();
        return Ok(categories.Select(MapToDto).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(int id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
        {
            return NotFound("Category not found");
        }

        return Ok(MapToDto(category));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> AddNewCategory(CreateCategoryRequest request)
    {
        var category = new Category
        {
            CategoryName = request.CategoryName,
            Description = request.Description
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCategoryById),
            new { id = category.CategoryId },
            MapToDto(category));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryResponseDto>> UpdateCategory(int id, CreateCategoryRequest request)
    {
        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (existingCategory == null)
        {
            return NotFound("Category not found");
        }

        existingCategory.CategoryName = request.CategoryName;
        existingCategory.Description = request.Description;

        await _context.SaveChangesAsync();

        return Ok(MapToDto(existingCategory));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
        {
            return NotFound("Category not found");
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static CategoryResponseDto MapToDto(Category category)
    {
        return new CategoryResponseDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description
        };
    }
}