using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkExercise.Models;

[Table("seller")]
public class Seller
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("uuid")]
    public Guid Uuid { get; set; } = Guid.NewGuid();

    [Column("name")]
    public string Name { get; set; } = string.Empty!;

    public List<Sale> Sales { get; set; } = default!;
}