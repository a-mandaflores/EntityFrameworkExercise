using EntityFrameworkExercise.ViewModel.Product;

namespace EntityFrameworkExercise.ViewModel.Sales
{
    public class ProductsForSaleResponse
    {
        public required List<ProductReadResponse> Products { get; set; }

    }
}
