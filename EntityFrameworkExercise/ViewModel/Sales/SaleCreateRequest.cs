namespace EntityFrameworkExercise.ViewModel.Sales
{
    public class SaleCreateRequest
    {

        public SellerCreateRequest Seller {  get; set; }
        public CustomerCreateRequest Customer { get; set; }
        public List<ProductCreateRequest> Products { get; set; }

    }
}
