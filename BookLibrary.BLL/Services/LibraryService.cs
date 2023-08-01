using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;

namespace BookLibrary.BLL.Services;

public abstract class LibraryService<T, U> : ILibraryService<T> where T : class where U : class, IEntity
{
    internal readonly IRepository<U> _repository;

    protected LibraryService(IRepository<U> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<T?>?> GetAllAsync()
    {
        return MapToModel(await _repository.GetAllAsync());
    }

    public async Task<T?> GetAsync(int id)
    {
        return MapToModel(await _repository.GetAsync(id));
    }

    public async Task<T?> AddAsync(T entity)
    {
        return MapToModel(await _repository.AddAsync(MapToEntity(entity)));
    }

    public async Task<T?> UpdateAsync(T entity)
    {
        return MapToModel(await _repository.UpdateAsync(MapToEntity(entity)));
    }

    public Task DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    internal abstract T? MapToModel(U? entity);
    internal abstract U? MapToEntity(T? entity);
    internal abstract IEnumerable<T?>? MapToModel(IEnumerable<U?>? entities);
    internal abstract IEnumerable<U?>? MapToEntity(IEnumerable<T?>? entities);

    public void Dispose()
    {
        _repository.Dispose();
    }

    ~LibraryService()
    {
        Dispose();
    }
}