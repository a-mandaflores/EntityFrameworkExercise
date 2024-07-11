using Bogus.DataSets;
using EntityFrameworkExercise.Data;
using EntityFrameworkExercise.Models;
using EntityFrameworkExercise.ViewModel.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace EntityFrameworkExercise.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(StoreContext context) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Lista todos os elementos", Description = "Retorna uma lista com todos os clientes e a quantidade de compras")]
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customerResponses = await context.Customers
            .Select(x => new CustomerReadResponse
            {
                Id = x.Uuid,
                Name = x.Name,
                Sale = x.Sales.Count
            })
            .ToListAsync();

            return Ok(customerResponses);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Lista apenas um cliente", Description = "Passamos o id pela rota e retorna o cliente especifico")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        var customer = await context.Customers
            .Where(x => x.Uuid == id)
            .Select(x => new CustomerReadResponse
            {
                Id = x.Uuid,
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
    [SwaggerOperation(Summary = "Editar dados do cliente", Description = "Edita os dados especificos do cliente")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(Guid id, CustomerUpdateRequest edit)
    {
        var customer = await context.Customers.Where(x => x.Uuid == id).FirstOrDefaultAsync(); ;
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
    [SwaggerOperation(Summary = "Criar cliente", Description = "Este metodo é responsável pela criação dos clientes")]
    [HttpPost]
    public async Task<IActionResult> PostCustomer(CustomerCreateRequest create)
    {
        var customer = new Customer
        {
           
            Name = create.Name,
        };

        context.Customers.Add(customer);

        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Uuid }, null);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Excluir cliente", Description = "Faz a exclusão do cliente que passarmos por rota")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        var customer = await context.Customers.Where(x => x.Uuid == id).SingleOrDefaultAsync();

        if (customer == null) 
        { 
            return NotFound(); 
        }

        context.Customers.Remove(customer);

        await context.SaveChangesAsync();

        return NoContent(); 
    }

   
}