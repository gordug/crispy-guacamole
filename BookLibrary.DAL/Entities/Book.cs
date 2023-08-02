using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.DAL.Entities;

public class Book : IValidatableObject, IEntity
{
    public Book(
        string title,
        List<Author?>? authors,
        List<Genre?>? genres,
        string isbn,
        int publicationYear)
    {
        Title = title;
        Authors = authors;
        Genres = genres;
        Isbn = isbn;
        PublicationYear = publicationYear;
    }
    
    public Book() { }

    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("BookId")]
    public int ID { get; init; }

    [Required,MaxLength(50), MinLength(1)]  public string Title { get; set; } = string.Empty;
    public List<Author?>? Authors { get; set; }
    [Required, MaxLength(50), MinLength(1)]
    public List<Genre?>? Genres { get; set; }
    [Required,MaxLength(13)]  public string Isbn { get; init; } = string.Empty;
    [Required,Range(1000, 9999)]  public int PublicationYear { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PublicationYear is < 1000 or > 9999)
            yield return new ValidationResult("Publication year must be between 1000 and 9999",
                new[] { nameof(PublicationYear) });

        if (Isbn.Length != 13)
            yield return new ValidationResult("ISBN must be 13 characters long", new[] { nameof(Isbn) });
    }
}