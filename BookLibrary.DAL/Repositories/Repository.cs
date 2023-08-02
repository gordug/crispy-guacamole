using BookLibrary.DAL.Entities;

namespace BookLibrary.DAL.Repositories;

internal abstract class Repository<T> : IRepository<T> where T : class, IEntity
{
    internal readonly LibraryContext Context;

    protected Repository(LibraryContext context)
        => Context = context;

    public virtual Task<IEnumerable<T?>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<T?>>(Context.Set<T>());
    }

    public virtual async ValueTask<T?> GetAsync(int id)
    {
        return await Context.FindAsync<T>(id);
    }

    public virtual async Task<T?> AddAsync(T? entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await Context.Set<T>().AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T?> UpdateAsync(T entity)
    {
        var entityToUpdate = await Context.FindAsync<T>(entity.ID).ConfigureAwait(false);
        if (entityToUpdate is null)
        {
            return entityToUpdate;
        }

        Context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T?> DeleteAsync(int id)
    {
        var entity = await Context.FindAsync<T>(id).ConfigureAwait(false);
        if (entity is null)
        {
            return entity;
        }

        entity = Context.Set<T>().Remove(entity).Entity;
        await Context.SaveChangesAsync();
        return entity;
    }

    public void Dispose()
    {
        Context.Dispose();
    }

    ~Repository()
    {
        Dispose();
    }
}