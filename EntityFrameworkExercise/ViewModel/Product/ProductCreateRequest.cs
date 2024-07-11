namespace EntityFrameworkExercise.ViewModel.Product
{
    public class ProductCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = default!;
    }
}
