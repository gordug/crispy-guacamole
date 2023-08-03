using BookLibrary.DAL.Entities;

namespace BookLibrary.DAL.Repositories;

internal abstract class Repository<T> : IRepository<T> where T : class, IEntity
{
    internal readonly LibraryContext Context;

    internal Repository(LibraryContext context)
        => Context = context;

    public virtual ValueTask<IEnumerable<T?>> GetAllAsync()
    {
        return ValueTask.FromResult<IEnumerable<T?>>(Context.Set<T>());
    }

    public virtual async ValueTask<T?> GetAsync(int id)
    {
        return await ValueTask.FromResult(Context.Find<T>(id));
    }

    public virtual ValueTask<T?> AddAsync(T? entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        Context.Set<T>().Add(entity);
        Context.SaveChanges();
        return ValueTask.FromResult<T?>(entity);
    }

    public virtual ValueTask<T?> UpdateAsync(T? entity)
    {
        if (entity is null)
        {
            return ValueTask.FromResult<T?>(default);
        }

        var entityToUpdate = Context.Find<T>(entity.ID);
        if (entityToUpdate is null)
        {
            return ValueTask.FromResult(entityToUpdate);
        }

        Context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
        Context.SaveChanges();
        return ValueTask.FromResult<T?>(entity);
    }

    public virtual async ValueTask<T?> DeleteAsync(int id)
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
