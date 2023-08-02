using System.Runtime.CompilerServices;
using BookLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

// InternalsVisibleTo is required for unit testing
[assembly: InternalsVisibleTo("BookLibrary.DAL.Tests")]

namespace BookLibrary.DAL.Repositories;

internal class BookRepository : Repository<Book>
{

    public BookRepository(LibraryContext context) : base(context) { }

    public override async ValueTask<Book?> GetAsync(int id)
    {
        return await Context.Books!
                            .Include<Book, List<Author?>?>(book => book!.Authors)
                            .Include(book => book.Genres)
                            .FirstOrDefaultAsync(book => book.ID == id);
    }
    
    public override async Task<IEnumerable<Book?>> GetAllAsync()
    {
        return await Context.Books!
                            .Include<Book, List<Author?>?>(book => book!.Authors)
                            .Include(book => book.Genres)
                            .ToListAsync();
    }

    public override Task<Book?> AddAsync(Book? entity)
    {
        if (entity is null) return Task.FromResult<Book?>(null);
        var book = Context.Books!.Add(new Book
        (
            entity.Title,
            entity.Authors?.Select(author => Context.Authors.Find(author.ID)).ToList(),
            entity.Genres?.Select(genre => Context.Genres.Find(genre.ID)).ToList(),
            entity.Isbn,
            entity.PublicationYear
        ));
        Context.SaveChanges();
        return Task.FromResult(book.Entity);
    }
}