using BookLibrary.BLL.Services;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using BookLibrary.Models;
using Moq;

namespace BookLibrary.BLL.Tests;

public class GenreServiceTests
{
    private readonly Mock<IRepository<Genre>> _genreRepositoryMock;

    public GenreServiceTests()
    {
        _genreRepositoryMock = new Mock<IRepository<Genre>>();
    }

    [SetUp]
    public void Setup()
    {
        _genreRepositoryMock.Reset();
    }

    private List<Genre> GetGenres()
    {
        return new List<Genre>
        {
            new() { ID = 1, Name = "Genre 1" },
            new() { ID = 2, Name = "Genre 2" },
            new() { ID = 3, Name = "Genre 3" }
        };
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllGenres()
    {
        // Arrange
        var genres = GetGenres();

        _genreRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(genres);

        var genreService = new GenreService(_genreRepositoryMock.Object);

        // Act
        var result = (await genreService.GetAllAsync() ?? Array.Empty<GenreModel?>()).ToArray();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Length.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(result[0]?.Id, Is.EqualTo(1));
            Assert.That(result[0]?.Name, Is.EqualTo("Genre 1"));
            Assert.That(result[1]?.Id, Is.EqualTo(2));
            Assert.That(result[1]?.Name, Is.EqualTo("Genre 2"));
            Assert.That(result[2]?.Id, Is.EqualTo(3));
            Assert.That(result[2]?.Name, Is.EqualTo("Genre 3"));
        });
    }

    [Test]
    public async Task GetAllAsync_ReturnsEmptyArray_WhenGenresNotFound()
    {
        // Arrange
        _genreRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(Array.Empty<Genre>());

        var genreService = new GenreService(_genreRepositoryMock.Object);

        // Act
        var result = (await genreService.GetAllAsync() ?? Array.Empty<GenreModel?>()).ToArray();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAsync_ReturnsGenre_WhenGenreExists()
    {
        // Arrange
        var genre = new Genre { ID = 1, Name = "Genre 1" };

        _genreRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(genre);

        var genreService = new GenreService(_genreRepositoryMock.Object);

        // Act
        var result = await genreService.GetAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result?.Id, Is.EqualTo(1));
            Assert.That(result?.Name, Is.EqualTo("Genre 1"));
        });
    }

    [Test]
    public async Task GetAsync_ReturnsNull_WhenGenreNotExists()
    {
        // Arrange
        _genreRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((Genre?)null);

        var genreService = new GenreService(_genreRepositoryMock.Object);

        // Act
        var result = await genreService.GetAsync(1);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateAsync_ReturnsCreatedGenre()
    {
        // Arrange
        var genre = new Genre { ID = 1, Name = "Genre 1" };

        _genreRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Genre>())).ReturnsAsync(genre);

        var genreService = new GenreService(_genreRepositoryMock.Object);

        // Act
        var result = await genreService.AddAsync(new GenreModel("Genre 1"));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result?.Id, Is.EqualTo(1));
            Assert.That(result?.Name, Is.EqualTo("Genre 1"));
        });
    }

    [Test]
    public async Task UpdateAsync_ReturnsUpdatedGenre()
    {
        // Arrange
        var genre = new Genre { ID = 1, Name = "Genre 1" };

        _genreRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Genre>())).ReturnsAsync(genre);

        var genreService = new GenreService(_genreRepositoryMock.Object);

        // Act
        var result = await genreService.UpdateAsync(new GenreModel("Genre 1"));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result?.Id, Is.EqualTo(1));
            Assert.That(result?.Name, Is.EqualTo("Genre 1"));
        });
    }

    [Test]
    public async Task DeleteAsync_CallsDeleteOnRepository()
    {
        // Arrange
        const int genreId = 1;
        _genreRepositoryMock.Setup(s => s.GetAsync(genreId)).ReturnsAsync(() => GetGenres().First());
        _genreRepositoryMock.Setup(s => s.DeleteAsync(genreId)).Returns(() => Task.FromResult(GetGenres().First()));
        var genreService = new GenreService(_genreRepositoryMock.Object);


        // Act
        await genreService.DeleteAsync(1);

        // Assert
        _genreRepositoryMock.Verify(x => x.DeleteAsync(genreId), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_CallsDeleteOnRepositoryWithCorrectId()
    {
        // Arrange
        const int genreId = 1;
        _genreRepositoryMock.Setup(s => s.GetAsync(genreId)).ReturnsAsync(() => GetGenres().First());
        _genreRepositoryMock.Setup(s => s.DeleteAsync(genreId)).Returns(() => Task.FromResult(GetGenres().First()));
        var genreService = new GenreService(_genreRepositoryMock.Object);


        // Act
        await genreService.DeleteAsync(1);

        // Assert
        _genreRepositoryMock.Verify(x => x.DeleteAsync(genreId), Times.Once);
    }

    [Test]
    public void MapToModel_ReturnsCorrectModel()
    {
        // Arrange
        var genre = new Genre { ID = 1, Name = "Genre 1" };
        var genreService = new GenreService(_genreRepositoryMock.Object);

        // Act
        var result = genreService.MapToModel(genre);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result?.Id, Is.EqualTo(1));
            Assert.That(result?.Name, Is.EqualTo("Genre 1"));
        });
    }

    [Test]
    public void MapToModel_ReturnsNull_WhenGenreIsNull()
    {
        // Arrange
        var genreService = new GenreService(_genreRepositoryMock.Object);

        // Act
        var result = genreService.MapToModel((Genre)null);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void MapToEntity_ReturnsCorrectEntity()
    {
        // Arrange
        var genre = new GenreModel("Genre 1");
        var genreService = new GenreService(_genreRepositoryMock.Object);

        // Act
        var result = genreService.MapToEntity(genre);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result?.ID, Is.EqualTo(0));
            Assert.That(result?.Name, Is.EqualTo("Genre 1"));
        });
    }

    [Test]
    public void MapToEntity_ReturnsNull_WhenGenreIsNull()
    {
        // Arrange
        var genreService = new GenreService(_genreRepositoryMock.Object);

        // Act
        var result = genreService.MapToEntity((GenreModel)null);

        // Assert
        Assert.That(result, Is.Null);
    }
}