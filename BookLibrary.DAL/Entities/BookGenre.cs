using System.ComponentModel.DataAnnotations;

namespace BookLibrary.DAL.Entities;

public class BookGenre
{
    public int BookID { get; set; }
    public Book Book { get; set; } = default!;
    public int GenreID { get; set; }
    public Genre Genre { get; set; } = default!;
}
