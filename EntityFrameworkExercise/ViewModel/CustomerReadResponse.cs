namespace EntityFrameworkExercise.ViewModel
{
    public class CustomerReadResponse
    {
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = string.Empty!;
        public int Sale { get; set; } = default!;


    }
}
