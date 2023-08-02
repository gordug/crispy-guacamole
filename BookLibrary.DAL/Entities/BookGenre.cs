using System.ComponentModel.DataAnnotations;

namespace BookLibrary.DAL.Entities;

public class BookGenre : IValidatableObject
{
    
    public int BookID { get; set; }
    public Book Book { get; set; } = default!;
    public int GenreID { get; set; }
    public Genre Genre { get; set; } = default!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (BookID == 0) yield return new ValidationResult("Book ID must be greater than 0", new[] { nameof(BookID) });

        if (GenreID == 0)
            yield return new ValidationResult("Genre ID must be greater than 0", new[] { nameof(GenreID) });
    }
}