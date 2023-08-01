using System.ComponentModel.DataAnnotations;

namespace BookLibrary.DAL.Entities;

public class BookAuthor : IValidatableObject
{
    public int BookID { get; set; }
    public Book Book { get; set; } = default!;
    public int AuthorID { get; set; }
    public Author Author { get; set; } = default!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (BookID == AuthorID)
            yield return new ValidationResult("BookID and AuthorID cannot be the same",
                new[] { nameof(BookID), nameof(AuthorID) });
    }
}