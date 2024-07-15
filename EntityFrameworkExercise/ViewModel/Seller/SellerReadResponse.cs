using EntityFrameworkExercise.Models;

namespace EntityFrameworkExercise.ViewModel.Seller
{
    public class SellerReadResponse
    {
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = default!;

        public int Sales { get; set; } 

    }
}
