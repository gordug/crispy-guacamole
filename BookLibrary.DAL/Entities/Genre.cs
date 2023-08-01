using System.ComponentModel.DataAnnotations;

namespace BookLibrary.DAL.Entities;

public class Genre : IValidatableObject
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
            yield return new ValidationResult("Genre name is required", new[] { nameof(Name) });
    }
}