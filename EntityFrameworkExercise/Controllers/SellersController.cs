using EntityFrameworkExercise.Data;
using EntityFrameworkExercise.Models;
using EntityFrameworkExercise.ViewModel.Seller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EntityFrameworkExercise.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SellersController(StoreContext context) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Lista dos vendedores", Description = "Retorna uma lista com todos os vendedores")]
    [HttpGet]
    public async Task<IActionResult> GetSellers()
    {
        var sellers = await context.Sellers
            .Select(x => new SellerReadResponse
            {
                Id = x.Uuid,
                Name = x.Name,
                Sales = x.Sales.Count,
            })
            .ToListAsync();

        if (sellers == null)
        {
            return NotFound();
        }
        return Ok(sellers);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Lista e um vendedor", Description = "Retorna um vendedor pelo Id")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSeller(Guid id)
    {
        var seller = await context.Sellers
            .Where(x => x.Uuid == id)
            .Select(x => new SellerReadResponse
            {
                Id = x.Uuid,
                Name = x.Name,
                Sales = x.Sales.Count,
            })
            .FirstOrDefaultAsync();
        if (seller == null)
        {
            return NotFound();
        }

        return Ok(seller);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Vendas por vendedor", Description = "Metodo que tras todas as vendas de um vendedor")]
    [HttpGet("{id}/Sales")]
    public async Task<IActionResult> GetSalesForSellers(Guid id)
    {
        var seller = await context.Sellers
            .Where(x => x.Uuid == id)
            .Select(x => new SalesForSallersReadResponse
            {
                Name = x.Name,
                Sales = x.Sales,
            })
            .FirstOrDefaultAsync();

        if (seller == null)
        {
            return NotFound();
        }

        return Ok(seller);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Editar produto", Description = "Faz a edição do cadastro do vendedor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSeller(Guid id, SellerUpdateRequest update)
    {
        var seller = await context.Sellers
            .Where(x => x.Uuid == id)
            .FirstOrDefaultAsync();
        if (seller == null)
        {
            return NotFound();
        }

        seller.Name = update.Name;

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

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerOperation(Summary = "Criar vendedor", Description = "Metodo para criação do vendedor")]
    [HttpPost]
    public async Task<IActionResult> PostSeller(SellerCreateRequest create)
    {
        var seller = new Seller
        {
            Name = create.Name,
        };

        context.Sellers.Add(seller);

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

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Deletar produto", Description = "Metodo para deletar um vendedor")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSeller(Guid id)
    {
        var seller = await context.Sellers
            .Where(x => x.Uuid == id)
            .SingleOrDefaultAsync();

        if (seller == null)
        {
            return NotFound();
        }

        context.Sellers.Remove(seller);

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
}