using EntityFrameworkExercise.Data;
using EntityFrameworkExercise.Models;
using EntityFrameworkExercise.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EntityFrameworkExercise.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(StoreContext context) : ControllerBase
{
    // GET: api/Products
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

    // GET: api/Products/5
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

    // PUT: api/Products/5
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

    // POST: api/Products
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

    // DELETE: api/Products/5
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