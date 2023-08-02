using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.DAL.Entities;

public class Genre : IValidatableObject, IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("GenreId")]
    public int ID { get; init; }
    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
            yield return new ValidationResult("Genre name is required", new[] { nameof(Name) });
    }
}