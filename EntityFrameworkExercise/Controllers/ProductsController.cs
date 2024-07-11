using EntityFrameworkExercise.Data;
using EntityFrameworkExercise.Models;
using EntityFrameworkExercise.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;

namespace EntityFrameworkExercise.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(StoreContext context) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Lista dos produtos", Description = "Retorna uma lista com todos os produtos")]
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await context.Products
            .Select(x => new ProductReadResponse()
            {
                Uuid = x.Uuid,
                Name = x.Name,
                Price = x.Price,
            })
            .ToListAsync();

        if(products == null)
        {
            return NotFound();
        }

        return Ok(products);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Listar um porduto", Description = "Lista o produto conforme o id passado pela URL")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await context.Products
            .Where(x => x.Uuid == id)
            .Select (x => new ProductReadResponse
            {
                Uuid = x.Uuid,
                Name = x.Name,
                Price = x.Price,
            })
            .FirstOrDefaultAsync();

        if (product == null) { 
          return NotFound();
        }

        return Ok(product);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Editar produto", Description = "Faz a edição do cadastro do produto")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(Guid id, ProductUpdateRequest update)
    {
        var product = await context.Products.Where(x => x.Uuid == id).FirstOrDefaultAsync();

        if (product == null) {
            return NotFound();
        }

        product.Name = update.Name; 
        product.Price = update.Price;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex) { 
            return BadRequest(ex.Message);
        }

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerOperation(Summary = "Criar produto", Description = "Metodo para criação do produto")]
    [HttpPost]
    public async Task<IActionResult> PostProduct(ProductCreateRequest create)
    {
        var product = new Product
        {
            Name = create.Name,
            Price = create.Price,
        };

        context.Products.Add(product);
       
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Deletar produto", Description = "Metodo para deletar um produto")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await context.Products.Where(x => x.Uuid == id).SingleOrDefaultAsync();

        if ( product == null) { return NotFound(); }

        context.Products.Remove(product);
        try
        {
            await context.SaveChangesAsync();
        }catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }
}