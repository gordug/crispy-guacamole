using BookLibrary.DAL.Entities;

namespace BookLibrary.DAL.Repositories;

public interface IRepository<T> : IDisposable where T : class, IEntity
{
    Task<IEnumerable<T?>> GetAllAsync();
    ValueTask<T?> GetAsync(int id);
    Task<T?> AddAsync(T? entity);
    Task<T?> UpdateAsync(T entity);
    Task<T?> DeleteAsync(int id);
}