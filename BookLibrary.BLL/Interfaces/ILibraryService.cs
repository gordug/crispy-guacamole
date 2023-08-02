namespace BookLibrary.BLL.Interfaces;

public interface ILibraryService<T> : IDisposable
{
    public Task<IEnumerable<T?>?> GetAllAsync();
    public Task<T?> GetAsync(int id);
    public Task<T?> AddAsync(T model);
    public Task<T?> UpdateAsync(T model);
    public Task DeleteAsync(int id);
}