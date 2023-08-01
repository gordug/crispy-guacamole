using System.Runtime.CompilerServices;
using BookLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

// InternalsVisibleTo is required for unit testing
[assembly: InternalsVisibleTo("BookLibrary.DAL.Tests")]

namespace BookLibrary.DAL.Repositories;

internal class AuthorRepository : IRepository<Author>
{
    private readonly LibraryContext _context;

    public AuthorRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author?>> GetAllAsync()
    {
        return await _context.Authors.ToListAsync();
    }

    public ValueTask<Author?> GetAsync(int id)
    {
        return _context.Authors.FindAsync(id);
    }

    public async Task<Author?> AddAsync(Author? author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<Author?> UpdateAsync(Author author)
    {
        var authorToUpdate = await _context.Authors.FindAsync(author.ID).ConfigureAwait(false);
        if (authorToUpdate is null) return authorToUpdate;
        _context.Entry(authorToUpdate).CurrentValues.SetValues(author);
        _context.Authors.Update(authorToUpdate);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<Author?> DeleteAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id).ConfigureAwait(false);
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
        return author;
    }
}