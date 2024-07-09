using EntityFrameworkExercise.Data;
using EntityFrameworkExercise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkExercise.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(StoreContext context) : ControllerBase
{
    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>?> GetProducts()
    {
        var products = await context.Products.ToListAsync();

        if(products.Count == 0)
        {
            return null;
        }

        return products;
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product == null) { 
          return NotFound();
        }

        return product;
    }

    // PUT: api/Products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, Product product)
    {
        if (product.Id != id) { 
            return NotFound();
        }

        context.Products.Entry(product).State = EntityState.Modified;

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
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
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
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if( product == null) { return NotFound(); }

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