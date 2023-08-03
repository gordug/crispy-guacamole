using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.DAL.Entities;

public class Genre : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("GenreId")]
    public virtual int ID { get; init; }

    [Required, MaxLength(50)]
    public virtual string Name { get; set; } = string.Empty;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
