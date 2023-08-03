using BookLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DAL;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book?> Books { get; set; } = default!;
    public virtual DbSet<Author?> Authors { get; set; } = default!;
    public virtual DbSet<Genre?> Genres { get; set; } = default!;
    public virtual DbSet<BookGenre> BookGenres { get; set; } = default!;
    public virtual DbSet<BookAuthor> BookAuthors { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookGenre>()
                    .HasKey(bg => new
                    {
                        bg.BookID,
                        bg.GenreID
                    });

        modelBuilder.Entity<BookAuthor>()
                    .HasKey(ba => new
                    {
                        ba.BookID,
                        ba.AuthorID
                    });
    }
}
