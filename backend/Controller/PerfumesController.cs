using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShopAPI.Models;
using PerfumeShopAPI.DTOs;
using PerfumeShopAPI.Data;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PerfumesController : ControllerBase
{
    private readonly PerfumeShopeContext _context;
    public PerfumesController(PerfumeShopeContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PerfumeResponseDto>>> GetPerfume(
     [FromQuery] string? search,
     [FromQuery] int? categoryId,
     [FromQuery] decimal? minPrice,
     [FromQuery] decimal? maxPrice,
     [FromQuery] string? sortBy,
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 10)
    {
        var query = _context.Perfumes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.PerfumeName.Contains(search) ||
                p.Brand.Contains(search));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        query = sortBy?.ToLower() switch
        {
            "price" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name" => query.OrderBy(p => p.PerfumeName),
            "name_desc" => query.OrderByDescending(p => p.PerfumeName),
            _ => query.OrderBy(p => p.PerfumeId)
        };

        var totalCount = await query.CountAsync();

        var perfumes = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        Response.Headers.Append("X-Total-Count", totalCount.ToString());
        Response.Headers.Append("X-Page", page.ToString());
        Response.Headers.Append("X-Page-Size", pageSize.ToString());

        return Ok(perfumes.Select(MapToDto).ToList());
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<PerfumeResponseDto>> GetPerfumeById(int id)
    {
        var perfume = await _context.Perfumes.FirstOrDefaultAsync(e => e.PerfumeId == id);
        if (perfume == null)
        {
            return NotFound("Nothing found");
        }
        return Ok(MapToDto(perfume));
    }

    [HttpPost]
    public async Task<ActionResult<PerfumeResponseDto>> AddNewPerfume(CreatePerfumeRequest request)
    {
        var perfume = new Perfume
        {
            PerfumeName = request.PerfumeName,
            Brand = request.Brand,
            CategoryId = request.CategoryId,
            SizeMl = request.SizeMl,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            Gender = request.Gender
        };

        await _context.Perfumes.AddAsync(perfume);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPerfumeById), new { id = perfume.PerfumeId }, MapToDto(perfume));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PerfumeResponseDto>> UpdatePerfume(int id, CreatePerfumeRequest request)
    {
        var perfume = await _context.Perfumes.FirstOrDefaultAsync(s => s.PerfumeId == id);

        if (perfume == null)
        {
            return NotFound("Not found");
        }

        perfume.PerfumeName = request.PerfumeName;
        perfume.Brand = request.Brand;
        perfume.CategoryId = request.CategoryId;
        perfume.SizeMl = request.SizeMl;
        perfume.Price = request.Price;
        perfume.StockQuantity = request.StockQuantity;
        perfume.Gender = request.Gender;

        await _context.SaveChangesAsync();

        return Ok(MapToDto(perfume));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePerfume(int id)
    {
        var perfume = await _context.Perfumes.FirstOrDefaultAsync(s => s.PerfumeId == id);

        if (perfume == null)
        {
            return NotFound("Not found");
        }

        _context.Perfumes.Remove(perfume);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static PerfumeResponseDto MapToDto(Perfume perfume)
    {
        return new PerfumeResponseDto
        {
            PerfumeId = perfume.PerfumeId,
            PerfumeName = perfume.PerfumeName,
            Brand = perfume.Brand,
            Price = perfume.Price
        };
    }
}