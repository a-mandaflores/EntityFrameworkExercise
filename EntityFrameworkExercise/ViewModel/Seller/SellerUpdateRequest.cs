using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkExercise.ViewModel.Seller
{
    public class SellerUpdateRequest
    {
        public string Name { get; set; } = string.Empty!;
    }
}
