using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkExercise.Data;
using EntityFrameworkExercise.Models;
using EntityFrameworkExercise.ViewModel.Sales;
using EntityFrameworkExercise.ViewModel.Seller;
using EntityFrameworkExercise.ViewModel.Customer;
using EntityFrameworkExercise.ViewModel.Product;
using System.Linq;

namespace EntityFrameworkExercise.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class SalesController(StoreContext context) : ControllerBase
    {
        // GET: api/Sales
        [HttpGet]
        public async Task<IActionResult> GetSales(int page = 1, int countElements = 5)
        {
            var skip = (page - 1) * countElements;
            var sales = await context.Sales
                .Select(x => new SalesReadResponse
                {
                    Id = x.Uuid,
                    Date = x.Date,
                    SellerId = x.SellerId,
                    Seller = new SellerReadResponse
                    {
                        Id = x.Seller.Uuid,
                        Name = x.Seller.Name
                    },
                    CustomerId = x.CustomerId,
                    Customer = new CustomerReadResponse
                    {
                        Id = x.Customer.Uuid,
                        Name = x.Customer.Name
                    },
                    Products = x.Products.Select(p => new ProductReadResponse
                    {
                        Id = p.Uuid,
                        Name = p.Name,
                        Price = p.Price
                    }).ToList()
                })
                .Skip(skip)
                .Take(countElements)
                .ToListAsync();

            return Ok(sales);
        }

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSale(int id)
        {
            return default;
        }

        // PUT: api/Sales/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, Sale sale)
        {
            return default;
        }

        // POST: api/Sales
        [HttpPost]
        public async Task<IActionResult> PostSale(SaleCreateRequest request)
        {
        var sale = new Sale
        {
            Date = DateTimeOffset.Now,
        };

        var seller = await context.Sellers.FirstOrDefaultAsync(s => s.Uuid == request.Seller.Uuid);
        var customer = await context.Customers.FirstOrDefaultAsync(s => s.Uuid == request.Seller.Uuid);

        var productsUuids = request.Products.ToList();
        var products = await context.Products
            .Where(p => productsUuids.Contains(p.Uuid))
            .ToListAsync();

        if (customer == null || seller == null)
        {
            return NotFound();
        }

        sale.Seller = seller;
        sale.Customer = customer;


        context.Sales.Add(sale);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return CreatedAtAction(nameof(GetSale), new { id = sale.Uuid }, sale);
    }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            return default;
        }
    }
