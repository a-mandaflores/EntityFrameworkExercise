using EntityFrameworkExercise.Models;

namespace EntityFrameworkExercise.ViewModel.Seller
{
    public class SalesForSallersReadResponse
    {
        public string Name { get; set; } = string.Empty!;
        public List<Sale> Sales { get; set; } = default!;
    }
}
