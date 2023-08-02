using BookLibrary.DAL.Entities;

namespace BookLibrary.BLL.Interfaces;

public interface IMapper<TModel, TEntity>
    where TModel : class
    where TEntity : class, IEntity
{
    public TModel? MapToModel(TEntity? entity);
    public TEntity? MapToEntity(TModel? model);
    public IEnumerable<TModel?>? MapToModel(IEnumerable<TEntity?>? entities);
    public IEnumerable<TEntity?>? MapToEntity(IEnumerable<TModel?>? models);
}