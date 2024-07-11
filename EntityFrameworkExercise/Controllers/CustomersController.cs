using Bogus.DataSets;
using EntityFrameworkExercise.Data;
using EntityFrameworkExercise.Models;
using EntityFrameworkExercise.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace EntityFrameworkExercise.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(StoreContext context) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CustomerReadResponse>))]
    [SwaggerOperation(Description = "Ola")]
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
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

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerReadResponse))]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
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

        if(customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> PostCustomer(CustomerCreateRequest create)
    {
        var customer = new Customer
        {
            Name = create.Name,
        };

        context.Customers.Add(customer);

        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, null);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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