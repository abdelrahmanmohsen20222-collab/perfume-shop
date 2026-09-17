using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShopAPI.Data;
using PerfumeShopAPI.DTOs;
using PerfumeShopAPI.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly PerfumeShopeContext _context;

    public CustomersController(PerfumeShopeContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAllCustomers()
    {
        var customers = await _context.Customers.ToListAsync();
        return Ok(customers.Select(MapToDto).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> GetCustomerById(int id)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer == null)
        {
            return NotFound("Customer not found");
        }

        return Ok(MapToDto(customer));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> AddNewCustomer(CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address
        };

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCustomerById),
            new { id = customer.CustomerId },
            MapToDto(customer));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> UpdateCustomer(int id, CreateCustomerRequest request)
    {
        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (existingCustomer == null)
        {
            return NotFound("Customer not found");
        }

        existingCustomer.FirstName = request.FirstName;
        existingCustomer.LastName = request.LastName;
        existingCustomer.Email = request.Email;
        existingCustomer.Phone = request.Phone;
        existingCustomer.Address = request.Address;

        await _context.SaveChangesAsync();

        return Ok(MapToDto(existingCustomer));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer == null)
        {
            return NotFound("Customer not found");
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static CustomerResponseDto MapToDto(Customer customer)
    {
        return new CustomerResponseDto
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email
        };
    }
}