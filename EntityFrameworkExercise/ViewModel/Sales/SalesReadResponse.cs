using EntityFrameworkExercise.Models;
using EntityFrameworkExercise.ViewModel.Customer;
using EntityFrameworkExercise.ViewModel.Product;
using EntityFrameworkExercise.ViewModel.Seller;
using System;
using System.Collections.Generic;

namespace EntityFrameworkExercise.ViewModel.Sales
{
    public class SalesReadResponse
    {

        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public int SellerId { get; set; }
        public SellerReadResponse Seller { get; set; } = default!;

        public int CustomerId { get; set; } = default!;

        public CustomerReadResponse Customer { get; set; } = default!;

        public int Products { get; set; } = default!;
    }
}
