using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DAL.Tests;

[TestFixture]
public class BookRepositoryTests
{
    [SetUp]
    public async Task Setup()
    {
        await SetupMockData();
    }

    private readonly BookRepository _repository;
    private readonly Book _testBook;

    public BookRepositoryTests()
    {
        // Test book object
        _testBook = new Book
        {
            ID = 1,
            Title = "Test Book",
            Authors = new List<Author>
            {
                new()
                {
                    ID = 11,
                    FirstName = "Test",
                    LastName = "Author"
                }
            },
            Genres = new List<Genre>
            {
                new()
                {
                    ID = 11,
                    Name = "Test Genre"
                }
            },
            Isbn = "123456789",
            PublicationYear = 2023
        };
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase("BookLibrary")
            .Options;

        // Instantiate Repository with mocked DbContext
        _repository = new BookRepository(new LibraryContext(options));
    }

    private async Task SetupMockData()
    {
        var book = await _repository.GetAsync(_testBook.ID);
        if (book is not null) return;
        await _repository.AddAsync(_testBook);
    }

    [Test]
    public async Task Add_ShouldAddNewBook()
    {
        // Arrange
        var book = new Book
        {
            ID = 2,
            Title = "Test Book 2",
            Authors = new List<Author>
            {
                new()
                {
                    ID = 12,
                    FirstName = "Test",
                    LastName = "Author 2"
                }
            },
            Genres = new List<Genre>
            {
                new()
                {
                    ID = 12,
                    Name = "Test Genre 2"
                }
            },
            Isbn = "987654321",
            PublicationYear = 2021
        };

        // Act
        var result = await _repository.AddAsync(book);

        // Assert
        Assert.That(result, Is.EqualTo(book));

        // Cleanup
        await _repository.DeleteAsync(book.ID);
    }

    [Test]
    public async Task Get_ShouldReturnCorrectBook()
    {
        // Act
        var result = await _repository.GetAsync(_testBook.ID);

        // Assert
        Assert.That(result, Is.EqualTo(_testBook));
    }

    [Test]
    public async Task Delete_ShouldRemoveSpecifiedBook()
    {
        // Act
        await _repository.DeleteAsync(_testBook.ID);
        var result = await _repository.GetAsync(_testBook.ID);
        // Assert
        Assert.That(result, Is.Null);

        // Cleanup
        await _repository.AddAsync(_testBook);
    }

    [Test]
    public async Task Update_ShouldUpdateSpecifiedBook()
    {
        // Arrange
        var book = new Book
        {
            ID = 3,
            Title = "Test Book 3",
            Authors = new List<Author>
            {
                new()
                {
                    ID = 13,
                    FirstName = "Test",
                    LastName = "Author 3"
                }
            },
            Genres = new List<Genre>
            {
                new()
                {
                    ID = 13,
                    Name = "Test Genre 3"
                }
            },
            Isbn = "987654321",
            PublicationYear = 2021
        };
        book = await _repository.AddAsync(book);
        // Act
        book.Title = "Updated Test Book";
        var result = await _repository.UpdateAsync(book);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Title, Is.EqualTo("Updated Test Book"));

        // Cleanup
        await _repository.DeleteAsync(book.ID);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllBooks()
    {
        // Act
        var result = (await _repository.GetAllAsync()).ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.First(), Is.EqualTo(_testBook));
            Assert.That(result, Has.Length.EqualTo(1));
        });
    }
}