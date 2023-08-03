using System.Runtime.CompilerServices;
using BookLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

// InternalsVisibleTo is required for unit testing
[assembly: InternalsVisibleTo("BookLibrary.DAL.Tests")]

namespace BookLibrary.DAL.Repositories;

internal class BookRepository : Repository<Book>
{
    public BookRepository(LibraryContext context)
        : base(context)
    {
    }

    public override ValueTask<Book?> GetAsync(int id)
    {
        return ValueTask.FromResult(Context.Books!
                                           .Include<Book, List<Author?>?>(book => book!.Authors)
                                           .Include(book => book.Genres)
                                           .FirstOrDefault(book => book.ID == id));
    }

    public override ValueTask<IEnumerable<Book?>> GetAllAsync()
    {
        return ValueTask.FromResult<IEnumerable<Book?>>(
                                                        Context.Books!
                                                               .Include<Book, List<Author?>?>(book => book!.Authors)
                                                               .Include(book => book.Genres)
                                                               .AsEnumerable());
    }

    public override ValueTask<Book?> AddAsync(Book? entity)
    {
        if (entity is null)
        {
            return ValueTask.FromResult<Book?>(null);
        }

        var authors = entity.Authors?.Select(author => Context.Authors.Find(author?.ID)).ToList();
        var genres = entity.Genres?.Select(genre => Context.Genres.Find(genre?.ID)).ToList();
        var book = Context.Books!.Add(new Book(
                                               entity.Title,
                                               authors,
                                               genres,
                                               entity.Isbn,
                                               entity.PublicationYear
                                              ));
        Context.SaveChanges();
        return ValueTask.FromResult(book.Entity);
    }

    public override ValueTask<Book?> UpdateAsync(Book? entity)
    {
        if (entity is null)
        {
            return ValueTask.FromResult<Book?>(null);
        }

        var book = Context.Books!.Find(entity.ID);
        if (book is null)
        {
            return ValueTask.FromResult<Book?>(null);
        }

        book.Title = entity.Title;
        book.Authors = entity.Authors?.Select(author => Context.Authors.Find(author?.ID)).ToList();
        book.Genres = entity.Genres?.Select(genre => Context.Genres.Find(genre?.ID)).ToList();
        book.Isbn = entity.Isbn;
        book.PublicationYear = entity.PublicationYear;
        Context.SaveChanges();
        return ValueTask.FromResult<Book?>(book);
    }
}
