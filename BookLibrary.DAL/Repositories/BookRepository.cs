using System.Runtime.CompilerServices;
using BookLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

// InternalsVisibleTo is required for unit testing
[assembly: InternalsVisibleTo("BookLibrary.DAL.Tests")]

namespace BookLibrary.DAL.Repositories;

internal class BookRepository : IRepository<Book>
{
    private readonly LibraryContext _context;

    public BookRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<Book?> AddAsync(Book? book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<IEnumerable<Book?>> GetAllAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async ValueTask<Book?> GetAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<Book?> UpdateAsync(Book book)
    {
        var bookToUpdate = await _context.Books.FindAsync(book.Id).ConfigureAwait(false);
        if (bookToUpdate is null) return bookToUpdate;
        _context.Entry(bookToUpdate).CurrentValues.SetValues(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book?> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id).ConfigureAwait(false);
        if (book is null) return book;
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return book;
    }
}