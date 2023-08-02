using Microsoft.EntityFrameworkCore;
using BookLibrary.DAL.Entities;
using Moq;

namespace BookLibrary.DAL.Tests;

public class MockDbSet<TEntity> where TEntity : class, IEntity, new()
{
    private Mock<DbSet<TEntity>> _mockSet = new();

    public MockDbSet()
    {
        _mockSet.Setup(m => m.FindAsync(It.IsAny<int>()))
                .Returns((int id) => ValueTask.FromResult((TEntity?)new TEntity{ID = id}));
        _mockSet.Setup(m => m.Add(It.IsAny<TEntity>()))
                .Returns((TEntity entity) => ValueTask.FromResult(entity));
        _mockSet.Setup(m => m.Remove(It.IsAny<TEntity>()))
                .Returns((TEntity entity) => ValueTask.FromResult(entity));
        _mockSet.Setup(m => m.Update(It.IsAny<TEntity>()))
                .Returns((TEntity entity) => ValueTask.FromResult(entity));
        _mockSet.Setup(m => m.AddAsync(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()))
                .Returns((TEntity entity) => ValueTask.FromResult(entity));
        _mockSet.Setup(m => m.Remove(It.IsAny<TEntity>()))
                .Returns((TEntity entity) => ValueTask.FromResult(entity));
        _mockSet.Setup(m => m.Add(It.IsAny<TEntity>()))
                .Returns((TEntity entity) => entity);
    }
    
    public Mock<DbSet<TEntity>> Object => _mockSet;
}
