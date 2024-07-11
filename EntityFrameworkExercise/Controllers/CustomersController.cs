using Bogus.DataSets;
using EntityFrameworkExercise.Data;
using EntityFrameworkExercise.Models;
using EntityFrameworkExercise.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EntityFrameworkExercise.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(StoreContext context) : ControllerBase
{

    // GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerReadResponse>>> GetCustomers()
    {
        var customerResponses = await context.Customers
            .Select(x => new CustomerReadResponse
            {
                Id = x.Id,
                Name = x.Name,
                Sale = x.Sales.Count
            })
            .ToListAsync();

            return Ok(customerResponses);
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerReadResponse>> GetCustomer(int id)
    {
        var customer = await context.Customers
            .Where(x => x.Id == id)
            .Select(x => new CustomerReadResponse
            {
                Id = x.Id,
                Name = x.Name,
                Sale = x.Sales.Count
            })
            .FirstOrDefaultAsync();

        return customer;
    }

    // PUT: api/Customers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(int id, CustomerUpdateRequest edit)
    {
        var customer = await context.Customers.FindAsync(id);
        if (customer == null) {
            return BadRequest();
        }

        customer.Name = edit.Name;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return NoContent();
    }

    // POST: api/Customers
    [HttpPost]
    public async Task<ActionResult<Customer>> PostCustomer(CustomerCreateRequest create)
    {
        var customer = new Customer
        {
            Name = create.Name,
        };

        context.Customers.Add(customer);

        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, null);
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await context.Customers.FindAsync(id);

        if (customer == null) 
        { 
            return NotFound(); 
        }

        context.Customers.Remove(customer);

        await context.SaveChangesAsync();

        return NoContent(); 
    }

   
}