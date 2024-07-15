using EntityFrameworkExercise.Data;
using EntityFrameworkExercise.Models;
using EntityFrameworkExercise.ViewModel;
using EntityFrameworkExercise.ViewModel.Customer;
using EntityFrameworkExercise.ViewModel.Sales;
using EntityFrameworkExercise.ViewModel.Seller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EntityFrameworkExercise.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalesController(StoreContext context) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Lista todos as vendas", Description = "Retorna uma lista com todos as vendas")]
    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> GetSales(int page = 1, int countElements = 5)
    {
        var skip = (page - 1) * countElements;
        var pageCount = await context.Sales.CountAsync();
        var sales = await context.Sales
            .OrderBy(x => x.Id)
            .Select(x => new SalesReadResponse
            {
                Id = x.Uuid,
                Date = x.Date,
                SellerId = x.SellerId,
                Seller = new SellerReadResponse
                {
                    Id = x.Seller.Uuid,
                    Name = x.Seller.Name,
                },
                CustomerId = x.CustomerId,
                Customer = new CustomerReadResponse
                {
                    Id = x.Customer.Uuid,
                    Name = x.Customer.Name
                },
                Products = x.Products.Count
            })
            .Skip(skip)
            .Take(countElements)
            .ToListAsync();

        var result = new ResultListResponse<SalesReadResponse>
        {
            Data = sales,
            PageCount = pageCount
        };

        return Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Lista apenas uma vendas", Description = "Passamos o id pela rota e retorna uma venda especifica")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSale(Guid id)
    {
        var sale = await context.Sales
            .Where(x => x.Uuid == id)
            .Select(x => new SalesReadResponse
            {
                Id = x.Uuid,
                Date = x.Date,
                SellerId = x.SellerId,
                Seller = new SellerReadResponse
                {
                    Id = x.Seller.Uuid,
                    Name = x.Seller.Name,
                },
                CustomerId = x.CustomerId,
                Customer = new CustomerReadResponse
                {
                    Id = x.Customer.Uuid,
                    Name = x.Customer.Name
                },
                Products = x.Products.Count
            })
            .FirstOrDefaultAsync();

        return Ok(sale);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Editar dados da vendas", Description = "Edita os dados especificos da venda")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSale(Guid id, SaleUpdateRequest request)
    {
        var sale = await context.Sales
            .Where (x => x.Uuid == id)
            .SingleOrDefaultAsync();

        if(sale == null)
        {
            return NotFound();
        }

        var sellerTask = context.Sellers.FirstOrDefaultAsync(x => x.Uuid == request.SellerId.Uuid);

        var customerTask = context.Customers.FirstOrDefaultAsync(x => x.Uuid == request.CustomerId.Uuid);

        var productsAll = request.Products.Select(p => p.Uuid);

        var productsTask = context.Products
            .Where(p => productsAll.Contains(p.Uuid))
            .ToListAsync();

        await Task.WhenAll(sellerTask, customerTask, productsTask);

        var seller = sellerTask.Result;
        var customer = customerTask.Result; 
        var product = productsTask.Result;

        if (product == null || seller == null || customer == null)
        {
            return NotFound();
        }

        sale.Customer = customer;
        sale.Seller = seller;
        sale.Products = product;

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
    [SwaggerOperation(Summary = "Criar venda", Description = "Este metodo é responsável pela criação das vendas")]
    [HttpPost]
    public async Task<IActionResult> PostSale(SaleCreateRequest request)
    {
        var sellerTask = context.Sellers.FirstOrDefaultAsync(s => s.Uuid == request.Seller.Uuid);

        var customerTask = context.Customers.FirstOrDefaultAsync(s => s.Uuid == request.Customer.Uuid);

        var productsUuids = request.Products.Select(x => x.Uuid);

        var productsTask = context.Products
            .Where(p => productsUuids.Contains(p.Uuid))
            .ToListAsync();

        await Task.WhenAll(sellerTask, customerTask, productsTask);

        var seller = sellerTask.Result;
        var customer = customerTask.Result;
        var products = productsTask.Result;

        if (customer == null || seller == null || products == null)
        {
            return NotFound();
        };

        var sale = new Sale
        {
            Date = DateTimeOffset.Now,
            Seller = seller,
            Customer = customer,
            Products = products,
        };

        context.Sales.Add(sale);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return CreatedAtAction(nameof(GetSale), new { id = sale.Uuid }, null);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Excluir venda", Description = "Faz a exclusão da venda que passarmos por rota")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSale(Guid id)
    {
        var sale = await context.Sales
            .Where(x => x.Uuid == id)
            .SingleOrDefaultAsync();

        if (sale == null)
        {
            return NotFound();
        }

        context.Sales.Remove(sale);

        try
        {
            await context.SaveChangesAsync();
        }catch (Exception error)
        {
            return BadRequest(error.Message);
        }

        return NoContent();
    }
}