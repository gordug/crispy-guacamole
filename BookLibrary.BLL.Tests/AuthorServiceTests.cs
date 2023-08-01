using BookLibrary.BLL.Services;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using BookLibrary.Models;
using Moq;

namespace BookLibrary.BLL.Tests;

internal class AuthorServiceTests
{
    private readonly Mock<IRepository<Author>> _authorRepositoryMock;

    public AuthorServiceTests()
        => _authorRepositoryMock = new Mock<IRepository<Author>>();

    [SetUp]
    public void Setup()
    {
        _authorRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Author>
            {
                new() { ID = 1, FirstName = "FirstName1", LastName = "LastName1" },
                new() { ID = 2, FirstName = "FirstName2", LastName = "LastName2" },
                new() { ID = 3, FirstName = "FirstName3", LastName = "LastName3" }
            });
        _authorRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Author { ID = id, FirstName = $"FirstName{id}", LastName = $"LastName{id}" });
        _authorRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Author>()))
            .ReturnsAsync((Author author) => author);
        _authorRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Author>()))
            .ReturnsAsync((Author author) => author);
        _authorRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Author { ID = id });

    }

    [Test]
    public async Task GetAllAsyncTest()
    {
        // Arrange
        var service = new AuthorService(_authorRepositoryMock.Object);

        // Act
        var authors = (await service.GetAllAsync() ?? Array.Empty<AuthorModel>()).ToArray();

        // Assert
        Assert.That(authors, Is.Not.Null);
        Assert.That(authors, Has.Length.EqualTo(3));
        Assert.Multiple(() =>
                        {
                            Assert.That(authors[0]?.Id, Is.EqualTo(1));
                            Assert.That(authors[0]?.FirstName, Is.EqualTo("FirstName1"));
                            Assert.That(authors[0]?.LastName, Is.EqualTo("LastName1"));
                            Assert.That(authors[1]?.Id, Is.EqualTo(2));
                            Assert.That(authors[1]?.FirstName, Is.EqualTo("FirstName2"));
                            Assert.That(authors[1]?.LastName, Is.EqualTo("LastName2"));
                            Assert.That(authors[2]?.Id, Is.EqualTo(3));
                            Assert.That(authors[2]?.FirstName, Is.EqualTo("FirstName3"));
                            Assert.That(authors[2]?.LastName, Is.EqualTo("LastName3"));
                        });
        
    }

    [Test]
    public async Task GetAsyncTest()
    {
        // Arrange
        var service = new AuthorService(_authorRepositoryMock.Object);

        // Act
        var author = await service.GetAsync(1);

        // Assert
        Assert.That(author, Is.Not.Null);
        Assert.Multiple(() =>
        {
                            Assert.That(author?.Id, Is.EqualTo(1));
                            Assert.That(author?.FirstName, Is.EqualTo("FirstName1"));
                            Assert.That(author?.LastName, Is.EqualTo("LastName1"));
                        });
    }

    [Test]
    public async Task AddAsyncTest()
    {
        // Arrange
        var service = new AuthorService(_authorRepositoryMock.Object);

        // Act
        var author = await service.AddAsync(new AuthorModel ("FirstName4", "LastName4" ));

        // Assert
        Assert.That(author, Is.Not.Null);
        Assert.Multiple(() =>
        {
                            Assert.That(author?.Id, Is.EqualTo(0));
                            Assert.That(author?.FirstName, Is.EqualTo("FirstName4"));
                            Assert.That(author?.LastName, Is.EqualTo("LastName4"));
                        });
    }

    [Test]
    public async Task UpdateAsyncTest()
    {
        // Arrange
        var service = new AuthorService(_authorRepositoryMock.Object);
        _authorRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Author>()))
                             .ReturnsAsync((Author _) => new Author{ID = 4, FirstName = "FirstName4", LastName = "LastName4"});

    // Act
        var author = await service.UpdateAsync(new AuthorModel (4, "FirstName4", "LastName4"));

        // Assert
        Assert.That(author, Is.Not.Null);
        Assert.Multiple(() =>
        {
                            Assert.That(author?.Id, Is.EqualTo(4));
                            Assert.That(author?.FirstName, Is.EqualTo("FirstName4"));
                            Assert.That(author?.LastName, Is.EqualTo("LastName4"));
                        });
    }

    [Test]
    public async Task DeleteAsyncTest()
    {
        // Arrange
        var service = new AuthorService(_authorRepositoryMock.Object);

        // Act
        await service.DeleteAsync(1);

        // Assert
        _authorRepositoryMock.Verify(x => x.DeleteAsync(It.Is<int>(id => id == 1)), Times.Once);
    }

}
