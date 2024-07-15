using Bogus;
using EntityFrameworkExercise.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkExercise.Data.Migrations
{
    /// <inheritdoc />
    public partial class MockEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            new Faker<Product>("pt_BR")
               .RuleFor(x => x.Name, x => x.Commerce.Product())
               .RuleFor(x => x.Price, x => x.Random.Decimal(0.0m, 100.1m))
               .Generate(100)
               .ForEach(p =>
               {
                   migrationBuilder.InsertData(
                       schema: "store",
                       table: "product",
                       columns: ["uuid", "name", "price"],
                       values: new object[,] { { Guid.NewGuid(), p.Name, p.Price } });
               });

            new Faker<Seller>("pt_BR")
                .RuleFor(x => x.Name, x => x.Person.FullName)
                .Generate(5)
                .ForEach(s =>
                {
                    migrationBuilder.InsertData(
                        schema: "store",
                        table: "seller",
                        columns: ["uuid", "name"],
                        values: new object[,] { { Guid.NewGuid(), s.Name } });
                });

            new Faker<Customer>("pt_BR")
                .RuleFor(x => x.Name, x => x.Person.FirstName)
                .Generate(5)
                .ForEach(c =>
                {
                    migrationBuilder.InsertData(
                        schema: "store",
                        table: "customer",
                        columns: ["uuid", "name"],
                        values: new object[,] { { Guid.NewGuid(), c.Name } });
                });

            new Faker<Sale>()
               .RuleFor(x => x.SellerId, x => x.Random.Int(1, 5))
               .RuleFor(x => x.CustomerId, x => x.Random.Int(1, 5))
               .RuleFor(x => x.Date, x => x.Date.Between(DateTime.Now.AddDays(-10), DateTime.Now))
               .Generate(100)
               .ForEach(s =>
               {
                   migrationBuilder.InsertData(
                      schema: "store",
                      table: "sale",
                      columns: ["uuid","date", "customer_id", "seller_id"],
                      values: new object[,] { { Guid.NewGuid(), s.Date, s.CustomerId, s.SellerId } });
               });
        }
        }
}
