using BookLibrary.DAL.Entities;

namespace BookLibrary.DAL.Repositories;

public interface IRepository<T> : IDisposable where T : class, IEntity
{
    ValueTask<IEnumerable<T?>> GetAllAsync();
    ValueTask<T?> GetAsync(int id);
    ValueTask<T?> AddAsync(T? entity);
    ValueTask<T?> UpdateAsync(T? entity);
    ValueTask<T?> DeleteAsync(int id);
}
