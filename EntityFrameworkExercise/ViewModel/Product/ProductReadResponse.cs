namespace EntityFrameworkExercise.ViewModel.Product
{
    public class ProductReadResponse
    {
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = default!;
    }
}
