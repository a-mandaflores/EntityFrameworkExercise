using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkExercise.Data;
using EntityFrameworkExercise.Models;
using EntityFrameworkExercise.ViewModel.Sales;
using EntityFrameworkExercise.ViewModel.Seller;
using EntityFrameworkExercise.ViewModel.Customer;
using EntityFrameworkExercise.ViewModel.Product;

namespace EntityFrameworkExercise.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class SalesController(StoreContext context) : ControllerBase
    {
        // GET: api/Sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
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
                .ToListAsync();

            return Ok(sales);
        }

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(int id)
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
        public async Task<ActionResult<Sale>> PostSale(Sale sale)
        {
            return default;
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            return default;
        }
    }
