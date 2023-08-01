using System.Runtime.CompilerServices;
using BookLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

// InternalsVisibleTo is required for unit testing
[assembly: InternalsVisibleTo("BookLibrary.DAL.Tests")]

namespace BookLibrary.DAL.Repositories;

internal class GenreRepository : IRepository<Genre>
{
    private readonly LibraryContext _context;

    public GenreRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre?>> GetAllAsync()
    {
        return await _context.Genres.ToListAsync();
    }

    public async ValueTask<Genre?> GetAsync(int id)
    {
        return await _context.Genres.FindAsync(id);
    }

    public async Task<Genre?> AddAsync(Genre? genre)
    {
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    public async Task<Genre?> UpdateAsync(Genre genre)
    {
        var genreToUpdate = await _context.Genres.FindAsync(genre.ID).ConfigureAwait(false);
        if (genreToUpdate is null) return genreToUpdate;
        _context.Entry(genreToUpdate).CurrentValues.SetValues(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    public async Task<Genre?> DeleteAsync(int id)
    {
        var genre = await _context.Genres.FindAsync(id).ConfigureAwait(false);
        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
        return genre;
    }
}