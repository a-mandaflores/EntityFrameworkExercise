using EntityFrameworkExercise.Models;

namespace EntityFrameworkExercise.ViewModel.Sales
{
    public class ProductSaleCreateRequest
    {
        public Guid ProductId { get; set; } = default!;

        public string Name { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public Sale Sale { get; set; } = default!;


    }
}
