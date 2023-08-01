using BookLibrary.DAL.Entities;

namespace BookLibrary.DAL.Repositories;

internal abstract class Repository<T> : IRepository<T> where T : class, IEntity
{
    private readonly LibraryContext _context;

    protected Repository(LibraryContext context)
        => _context = context;

    public async Task<IEnumerable<T?>> GetAllAsync()
    {
        return _context.Set<T>();
    }

    public async ValueTask<T?> GetAsync(int id)
    {
        return await _context.FindAsync<T>(id);
    }

    public async Task<T?> AddAsync(T? entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> UpdateAsync(T entity)
    {
        var entityToUpdate = await _context.FindAsync<T>(entity.ID).ConfigureAwait(false);
        if (entityToUpdate is null)
        {
            return entityToUpdate;
        }

        _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> DeleteAsync(int id)
    {
        var entity = await _context.FindAsync<T>(id).ConfigureAwait(false);
        if (entity is null)
        {
            return entity;
        }
        entity = _context.Set<T>().Remove(entity).Entity;
        await _context.SaveChangesAsync();
        return entity;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    ~Repository()
    {
        Dispose();
    }
}
