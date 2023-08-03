using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BookLibrary.DAL.Tests;

[TestFixture]
public class BookRepositoryTests
{
    [SetUp]
    public async Task Setup()
    {
    }

    private readonly Mock<DbSet<Book>> _mockBookSet;
    private readonly Mock<LibraryContext> _mockContext;
    private readonly BookRepository _repository;
    private const string AuthorFirstName = "Test";
    private const string AuthorLastName = "Author";
    private const string GenreName = "Test Genre";

    private static readonly Book TestBook = new()
    {
        ID = 1,
        Title = "Test Book",
        Authors = new List<Author?>
        {
            new()
            {
                ID = 1,
                FirstName = AuthorFirstName,
                LastName = AuthorLastName + " 1"
            }
        },
        Genres = new List<Genre?>
        {
            new()
            {
                ID = 1,
                Name = GenreName + " 1"
            }
        },
        Isbn = "123456789",
        PublicationYear = 2023
    };

    private readonly IQueryable<Author> _authors = new List<Author>
    {
        new()
        {
            ID = 1,
            FirstName = AuthorFirstName,
            LastName = AuthorLastName + " 1"
        },
        new()
        {
            ID = 2,
            FirstName = AuthorFirstName,
            LastName = AuthorLastName + " 2"
        },
        new()
        {
            ID = 3,
            FirstName = AuthorFirstName,
            LastName = AuthorLastName + " 3"
        }
    }.AsQueryable();

    private readonly IQueryable<Genre> _genres = new List<Genre>
    {
        new()
        {
            ID = 1,
            Name = GenreName + " 1"
        },
        new()
        {
            ID = 2,
            Name = GenreName + " 2"
        },
        new()
        {
            ID = 3,
            Name = GenreName + " 3"
        }
    }.AsQueryable();

    private readonly IQueryable<Book> _books = new List<Book> {TestBook}.AsQueryable();

    public BookRepositoryTests()
    {
        var authorSet = new MockDbSet<Author>(_authors);
        var genreSet = new MockDbSet<Genre>(_genres);
        var bookSet = new MockDbSet<Book>(_books);

        var authorSetMockEntity = authorSet.MockEntity;
        var genreSetMockEntity = genreSet.MockEntity;
        _mockBookSet = bookSet.MockEntity;
        var options = new DbContextOptionsBuilder<LibraryContext>()
                      .UseInMemoryDatabase("BookLibrary")
                      .Options;
        _mockContext = new Mock<LibraryContext>(options);
        _mockContext.Setup(m => m.Authors).Returns(authorSetMockEntity.Object!);
        _mockContext.Setup(m => m.Books).Returns(_mockBookSet.Object!);
        _mockContext.Setup(m => m.Genres).Returns(genreSetMockEntity.Object!);
        _repository = new BookRepository(_mockContext.Object);
    }

    [Test]
    public async Task Add_ShouldAddNewBook()
    {
        // Arrange

        // Act
        _ = await _repository.AddAsync(_books.First());

        // Assert
        _mockBookSet.Verify(m => m.Add(It.IsAny<Book>()), Times.Once);
        _mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Test]
    public async Task Get_ShouldReturnCorrectBook()
    {
        // Act
        _ = await _repository.GetAsync(TestBook.ID);

        // Assert
        _mockBookSet.Verify(m => m.Find(), Times.Once);
    }

    [Test]
    public async Task Delete_ShouldRemoveSpecifiedBook()
    {
        // Act
        await _repository.DeleteAsync(TestBook.ID);
        // Assert
        _mockBookSet.Verify(m => m.Remove(It.IsAny<Book>()), Times.Once);
        _mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Test]
    public async Task Update_ShouldUpdateSpecifiedBook()
    {
        // Arrange
        var book = new Book
        {
            ID = 3,
            Title = "Test Book 3",
            Authors = new List<Author?>
            {
                new()
                {
                    ID = 3,
                    FirstName = "Test",
                    LastName = "Author 3"
                }
            },
            Genres = new List<Genre?>
            {
                new()
                {
                    ID = 3,
                    Name = "Test Genre 3"
                }
            },
            Isbn = "987654321",
            PublicationYear = 2021
        };

        // Act
        _ = await _repository.UpdateAsync(book);

        // Assert
        _mockBookSet.Verify(m => m.Update(It.IsAny<Book>()), Times.Once);
        _mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllBooks()
    {
        // Act
        _ = (await _repository.GetAllAsync()).OrderBy(book => book?.ID).ToArray();

        // Assert
    }
}
