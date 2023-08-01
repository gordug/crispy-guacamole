namespace BookLibrary.BLL.Interfaces;

public interface ILibraryService<T> : IDisposable
{
    public Task<IEnumerable<T?>?> GetAllAsync();
    public Task<T?> GetAsync(int id);
    public Task<T?> AddAsync(T entity);
    public Task<T?> UpdateAsync(T entity);
    public Task DeleteAsync(int id);
}