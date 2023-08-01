using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.DAL.Entities;

public class Author : IValidatableObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required] [MaxLength(50)] public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            yield return new ValidationResult("First name is required", new[] { nameof(FirstName) });

        if (string.IsNullOrWhiteSpace(LastName))
            yield return new ValidationResult("Last name is required", new[] { nameof(LastName) });
    }
}