using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.DAL.Entities;

public class Book : IValidatableObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required] [MaxLength(200)] public string Title { get; set; } = string.Empty;
    public ICollection<Author> Authors { get; set; } = default!;
    public ICollection<Genre> Genres { get; set; } = default!;
    [Required] [MaxLength(13)] public string Isbn { get; init; } = string.Empty;
    [Required] [Range(1000, 9999)] public int PublicationYear { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PublicationYear is < 1000 or > 9999)
            yield return new ValidationResult("Publication year must be between 1000 and 9999",
                new[] { nameof(PublicationYear) });

        if (Isbn.Length != 13)
            yield return new ValidationResult("ISBN must be 13 characters long", new[] { nameof(Isbn) });
    }

    public object Clone()
    {
        return new Book
        {
            Id = Id,
            Title = Title,
            Authors = Authors,
            Genres = Genres,
            Isbn = Isbn,
            PublicationYear = PublicationYear
        };
    }
}