using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DAL.Entities;

public class Book : IEntity
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

    public Book()
    {
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("BookId")]
    public virtual int ID { get; init; }

    [Required, MaxLength(50), MinLength(1)]
    public virtual string Title { get; set; } = string.Empty;

    public virtual List<Author?>? Authors { get; set; }

    [Required, MaxLength(50), MinLength(1)]
    public virtual List<Genre?>? Genres { get; set; }

    [Required, MaxLength(13), MinLength(7)]
    public virtual string Isbn { get; set; } = string.Empty;

    [Required, Range(1000, 9999)]
    public virtual int PublicationYear { get; set; }

    public virtual void Configure(ModelBuilder builder)
    {
        builder.Entity<Book>().HasIndex(b => b.Isbn).IsUnique();
    }
}
