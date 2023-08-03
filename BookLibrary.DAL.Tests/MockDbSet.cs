using Microsoft.EntityFrameworkCore;
using BookLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using MockQueryable.EntityFrameworkCore;
using Moq;

namespace BookLibrary.DAL.Tests;

public class MockDbSet<TEntity> where TEntity : class, IEntity, new()
{
    public MockDbSet(IQueryable<TEntity> entities)
    {
        MockEntity.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(entities.Provider);
        MockEntity.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(entities.Expression);
        MockEntity.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(entities.ElementType);
        MockEntity.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
        MockEntity.Setup(m => m.Find(It.IsAny<object[]>()))
                  .Returns((object[] id) => entities.SingleOrDefault(e => e.ID == (int)id[0]));

        MockEntity.Setup(m => m.Add(It.IsAny<TEntity>()))
                  .Returns((TEntity entity) => new EntityEntry<TEntity?>(entity as InternalEntityEntry));
        MockEntity.Setup(m => m.Remove(It.IsAny<TEntity>()))
                  .Returns((TEntity entity) => new EntityEntry<TEntity?>(entity as InternalEntityEntry));
        MockEntity.Setup(m => m.Update(It.IsAny<TEntity>()))
                  .Returns((TEntity entity) => new EntityEntry<TEntity>(entity as InternalEntityEntry));
        var data = entities.AsQueryable();
        MockEntity.As<IAsyncEnumerable<TEntity>>()
                  .Setup(m => m.GetAsyncEnumerator(new CancellationToken()))
                  .Returns(new TestAsyncEnumerator<TEntity>(data.GetEnumerator()));
    }

    public Mock<DbSet<TEntity>> MockEntity { get; } = new();
}
