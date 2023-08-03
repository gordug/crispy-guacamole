using System.ComponentModel.DataAnnotations;

namespace BookLibrary.DAL.Entities;

public class BookAuthor
{
    public int BookID { get; set; }
    public Book Book { get; set; } = default!;
    public int AuthorID { get; set; }
    public Author Author { get; set; } = default!;
}
