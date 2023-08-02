using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;

namespace BookLibrary.BLL.Services;

public abstract class LibraryService<TModel, TEntity> : ILibraryService<TModel> where TModel : class where TEntity : class, IEntity
{
    internal readonly IRepository<TEntity> Repository;
    internal readonly IMapper<TModel, TEntity> Mapper;

    protected LibraryService(IRepository<TEntity> repository,
                             IMapper<TModel, TEntity> mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }

    public async Task<IEnumerable<TModel?>?> GetAllAsync()
    {
        return Mapper.MapToModel(await Repository.GetAllAsync());
    }

    public async Task<TModel?> GetAsync(int id)
    {
        return Mapper.MapToModel(await Repository.GetAsync(id));
    }

    public async Task<TModel?> AddAsync(TModel model)
    {
        return Mapper.MapToModel(await Repository.AddAsync(Mapper.MapToEntity(model)));
    }

    public async Task<TModel?> UpdateAsync(TModel model)
    {
        var entityToUpdate = Mapper.MapToEntity(model);
        return entityToUpdate is null ? null : Mapper.MapToModel(await Repository.UpdateAsync(entityToUpdate));
    }

    public Task DeleteAsync(int id)
    {
        return Repository.DeleteAsync(id);
    }

    
    public void Dispose()
    {
        Repository.Dispose();
    }

    ~LibraryService()
    {
        Dispose();
    }
}