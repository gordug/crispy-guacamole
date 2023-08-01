using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DAL.Tests;

public class GenreRepositoryTests
{
    private readonly GenreRepository _repository;
    private readonly Genre _testGenre;

    public GenreRepositoryTests()
    {
        // Test genre object
        _testGenre = new Genre
        {
            ID = 1,
            Name = "Test Genre"
        };
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase("BookLibrary")
            .Options;

        // Instantiate Repository with mocked DbContext
        _repository = new GenreRepository(new LibraryContext(options));
    }

    [SetUp]
    public async Task Setup()
    {
        await SetupMockData();
    }

    private async Task SetupMockData()
    {
        var genre = await _repository.GetAsync(_testGenre.ID);
        if (genre is not null) return;
        await _repository.AddAsync(_testGenre);
    }

    [Test]
    public async Task Add_ShouldAddNewGenre()
    {
        // Arrange
        var genre = new Genre
        {
            ID = 2,
            Name = "Test Genre 2"
        };

        // Act
        await _repository.AddAsync(genre);
        var result = await _repository.GetAsync(genre.ID);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ID, Is.EqualTo(genre.ID));
            Assert.That(result.Name, Is.EqualTo(genre.Name));
        });

        // Clean up
        await _repository.DeleteAsync(genre.ID);
    }

    [Test]
    public async Task Get_ShouldReturnGenre()
    {
        // Act
        var result = await _repository.GetAsync(_testGenre.ID);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ID, Is.EqualTo(_testGenre.ID));
            Assert.That(result.Name, Is.EqualTo(_testGenre.Name));
        });
    }

    [Test]
    public async Task GetAll_ShouldReturnAllGenres()
    {
        // Act
        var result = (await _repository.GetAllAsync()).OrderBy(genre => genre?.ID).ToArray();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Length.GreaterThan(0));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].ID, Is.EqualTo(_testGenre.ID));
            Assert.That(result[0].Name, Is.EqualTo(_testGenre.Name));
        });
    }

    [Test]
    public async Task Update_ShouldUpdateGenre()
    {
        // Arrange
        var genre = new Genre
        {
            ID = 2,
            Name = "Test Genre 2"
        };
        await _repository.AddAsync(genre);

        // Act
        genre.Name = "Updated Genre 2";
        var result = await _repository.UpdateAsync(genre);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ID, Is.EqualTo(genre.ID));
            Assert.That(result.Name, Is.EqualTo(genre.Name));
        });

        // Clean up
        await _repository.DeleteAsync(genre.ID);
    }
}