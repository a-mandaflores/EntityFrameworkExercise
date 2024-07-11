namespace EntityFrameworkExercise.ViewModel.Product
{
    public class ProductUpdateRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = default!;
    }
}
