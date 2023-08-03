using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DAL.Tests;

public class AuthorRepositoryTests
{
    private readonly AuthorRepository _repository;
    private readonly Author _testAuthor;

    public AuthorRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
                      .UseInMemoryDatabase("BookLibrary")
                      .Options;

        // Instantiate Repository with mocked DbContext
        _repository = new AuthorRepository(new LibraryContext(options));
        // Test author object
        _testAuthor = new Author
        {
            ID = 1,
            FirstName = "Test",
            LastName = "Author"
        };
    }

    [SetUp]
    public async Task Setup()
    {
        await SetupMockData();
    }

    private async Task SetupMockData()
    {
        var author = await _repository.GetAsync(_testAuthor.ID);
        if (author is { })
        {
            return;
        }

        await _repository.AddAsync(_testAuthor);
    }

    [Test]
    public async Task Add_ShouldAddNewAuthor()
    {
        // Arrange
        var author = new Author
        {
            ID = 2,
            FirstName = "Test",
            LastName = "Author 2"
        };

        // Act
        await _repository.AddAsync(author);

        // Assert
        var result = await _repository.GetAsync(author.ID);
        Assert.That(result, Is.EqualTo(author));

        // Cleanup
        await _repository.DeleteAsync(author.ID);
    }

    [Test]
    public async Task Get_ShouldReturnAuthor()
    {
        // Arrange
        var authorId = _testAuthor!.ID;

        // Act
        var result = await _repository.GetAsync(authorId);

        // Assert
        Assert.That(result, Is.EqualTo(_testAuthor));
    }

    [Test]
    public async Task Get_ShouldReturnNull()
    {
        // Arrange
        const int authorId = 2;

        // Act
        var result = await _repository.GetAsync(authorId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllAuthors()
    {
        // Arrange
        var author = new Author
        {
            ID = 2,
            FirstName = "Test",
            LastName = "Author 2"
        };
        await _repository.AddAsync(author);

        // Act
        var result = (await _repository.GetAllAsync()).ToArray();

        // Assert
        Assert.That(result, Has.Length.GreaterThan(1));
        Assert.That(result, Has.Exactly(1).Matches<Author>(a => a.ID == _testAuthor!.ID));
        Assert.That(result, Has.Exactly(1).Matches<Author>(a => a.ID == author.ID));

        // Cleanup
        await _repository.DeleteAsync(author.ID);
    }

    [Test]
    public async Task Update_ShouldUpdateAuthor()
    {
        // Arrange
        var author = new Author
        {
            ID = 2,
            FirstName = "Test",
            LastName = "Author 2"
        };
        await _repository.AddAsync(author);
        var updatedAuthor = new Author
        {
            ID = author.ID,
            FirstName = "Updated",
            LastName = "Author"
        };

        // Act

        await _repository.UpdateAsync(updatedAuthor);

        // Assert
        var result = await _repository.GetAsync(author.ID);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ID, Is.EqualTo(updatedAuthor.ID));
            Assert.That(result.FirstName, Is.EqualTo(updatedAuthor.FirstName));
            Assert.That(result.LastName, Is.EqualTo(updatedAuthor.LastName));
        });

        // Cleanup
        await _repository.DeleteAsync(_testAuthor.ID);
    }

    [Test]
    public async Task Delete_ShouldDeleteAuthor()
    {
        // Arrange
        var authorId = _testAuthor!.ID;

        // Act
        var deleteResult = await _repository.DeleteAsync(authorId);

        // Assert
        var result = await _repository.GetAsync(authorId);
        Assert.Multiple(() =>
        {
            Assert.That(deleteResult, Is.EqualTo(_testAuthor));
            Assert.That(result, Is.Null);
        });
    }
}
