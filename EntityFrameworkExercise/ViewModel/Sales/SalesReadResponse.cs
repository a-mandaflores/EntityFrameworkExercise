using EntityFrameworkExercise.Models;
using System;
using System.Collections.Generic;

namespace EntityFrameworkExercise.ViewModel.Sales
{
    public class SalesReadResponse
    {
        public Guid Uuid { get; set; } = default!;
        public DateTimeOffset Date { get; set; }
        public int SellerId { get; set; }
        public Seller Seller { get; set; } = default!;

        public int CustomerId { get; set; } = default!;

        public Customer Customer { get; set; } = default!;

        public List<Product> Products { get; set; } = default!;
    }
}
