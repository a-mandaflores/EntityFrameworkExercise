using EntityFrameworkExercise.Data;
using EntityFrameworkExercise.Models;
using EntityFrameworkExercise.ViewModel.Seller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkExercise.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SellersController(StoreContext context) : ControllerBase
{
    // GET: api/Sellers
    [HttpGet]
    public async Task<IActionResult> GetSellers()
    {
        var sellers = await context.Sellers
            .Select(x => new SellerReadResponse
            {
                Id = x.Uuid,
                Name = x.Name,
                Sales = x.Sales,
            })
            .ToListAsync();

        if (sellers == null)
        {
            return NotFound();
        }
        return Ok(sellers);
    }

    // GET: api/Sellers/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSeller(Guid id)
    {
        var seller = await context.Sellers
            .Where(x => x.Uuid == id)
            .Select(x => new SellerReadResponse
            {
                Id = x.Uuid,
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

    // PUT: api/Sellers/5
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

    // POST: api/Sellers
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

    // DELETE: api/Sellers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSeller(Guid id)
    {
        var seller = await context.Sellers
            .Where (x => x.Uuid == id)
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