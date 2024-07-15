namespace EntityFrameworkExercise.ViewModel.Sales
{
    public class SaleUpdateRequest
    {
        public SellerCreateRequest SellerId { get; set; } = default!;
        public CustomerCreateRequest CustomerId { get; set; } = default!;

        public List<ProductCreateRequest> Products { get; set; } = default!;

    }
}
