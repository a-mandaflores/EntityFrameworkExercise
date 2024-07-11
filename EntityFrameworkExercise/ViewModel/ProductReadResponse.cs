namespace EntityFrameworkExercise.ViewModel
{
    public class ProductReadResponse
    {
        public Guid Uuid { get; set; } = default!;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = default!;
    }
}
