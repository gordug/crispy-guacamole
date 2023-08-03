using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.DAL.Entities;

public class Author : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("AuthorId")]
    public virtual int ID { get; init; }

    [Required, MaxLength(50)]
    public virtual string FirstName { get; set; } = string.Empty;

    public virtual string LastName { get; set; } = string.Empty;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
