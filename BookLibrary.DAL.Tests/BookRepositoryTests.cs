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
    
    private readonly Mock<DbSet<Book>> _mockBookSet = new();
    private readonly Mock<DbSet<Author>> _mockAuthorSet = new();
    private readonly Mock<DbSet<Genre>> _mockGenreSet = new();
    private readonly Mock<LibraryContext> _mockContext = new();
    private readonly BookRepository _repository;
    private readonly Book _testBook;
    private readonly IEnumerable<Author> _authors;
    private readonly IEnumerable<Genre> _genres;

    public BookRepositoryTests()
    {
        var mockAuthorDbSets = new MockDbSet<Author>();
        _mockAuthorSet = mockAuthorDbSets.Object;
        var mockBookDbSets = new MockDbSet<Book>();
        _mockBookSet = mockBookDbSets.Object;
        var mockGenreDbSets = new MockDbSet<Genre>();
        _mockGenreSet = mockGenreDbSets.Object;
        _mockContext.Setup(m => m.Authors).Returns(_mockAuthorSet.Object);
        _mockContext.Setup(m => m.Books).Returns(_mockBookSet.Object);
        _mockContext.Setup(m => m.Genres).Returns(_mockGenreSet.Object);
        _repository = new BookRepository(_mockContext.Object);
        const string authorFirstName = "Test";
        const string authorLastName = "Author";
        const string genreName = "Test Genre";
        _authors = new List<Author>
        {
            new Author{ID = 1, FirstName = authorFirstName, LastName = authorLastName + " 1"},
            new Author{ID = 2, FirstName = authorFirstName, LastName = authorLastName + " 2"},
            new Author{ID = 3, FirstName = authorFirstName, LastName = authorLastName + " 3"}
        };
        _genres = new List<Genre>
        {
            new Genre
            {
                ID = 1,
                Name = genreName + " 1"
            },
            new Genre
            {
                ID = 2,
                Name = genreName + " 2"
            },
            new Genre
            {
                ID = 3,
                Name = genreName + " 3"
            }
        };


        // Test book object
        _testBook = new Book
            {
                ID = 1,
                Title = "Test Book",
                Authors = new List<Author?>
                {
                    new()
                    {
                        ID = 1,
                        FirstName = "Test",
                        LastName = "Author 1"
                    }
                },
                Genres = new List<Genre?>
                {
                    new()
                    {
                        ID = 1,
                        Name = "Test Genre 1"
                    }
                },
                Isbn = "123456789",
                PublicationYear = 2023
        };
    }
    
    [Test]
    public async Task Add_ShouldAddNewBook()
    {
        // Arrange
        var book = new Book
        {
            ID = 2,
            Title = "Test Book 2",
            Authors = new List<Author?>
            {
                new()
                {
                    ID = 2,
                    FirstName = "Test",
                    LastName = "Author 2"
                }
            },
            Genres = new List<Genre?>
            {
                new()
                {
                    ID = 2,
                    Name = "Test Genre 2"
                }
            },
            Isbn = "987654321",
            PublicationYear = 2021
        };

        // Act
        var result = await _repository.AddAsync(book);

        // Assert
        Assert.That (result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result?.ID, Is.EqualTo(book.ID));
            Assert.That(result?.Title, Is.EqualTo(book.Title));
            Assert.Multiple(() =>
            {
                Assert.That(result?.Authors, Is.Not.Null);
                Assert.That(result?.Authors?.Count, Is.EqualTo(book.Authors.Count));
                Assert.That(result?.Authors?.First()?.ID, Is.EqualTo(book.Authors.First()?.ID));
                Assert.That(result?.Authors?.First()?.FirstName, Is.EqualTo(book.Authors.First()?.FirstName));
                Assert.That(result?.Authors?.First()?.LastName, Is.EqualTo(book.Authors.First()?.LastName));

            });
            Assert.Multiple(() =>
            {
                Assert.That(result?.Genres, Is.Not.Null);
                Assert.That(result?.Genres?.Count, Is.EqualTo(book.Genres.Count));
                Assert.That(result?.Genres?.First()?.ID, Is.EqualTo(book.Genres.First()?.ID));
                Assert.That(result?.Genres?.First()?.Name, Is.EqualTo(book.Genres.First()?.Name));
            });
            Assert.That(result?.Isbn, Is.EqualTo(book.Isbn));
            Assert.That(result?.PublicationYear, Is.EqualTo(book.PublicationYear));
        });
    }

    [Test]
    public async Task Get_ShouldReturnCorrectBook()
    {
        // Act
        var result = await _repository.GetAsync(_testBook.ID);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result?.ID, Is.EqualTo(_testBook.ID));
            Assert.That(result?.Title, Is.EqualTo(_testBook.Title));
            Assert.Multiple(() =>
            {
                Assert.That(result?.Authors, Is.Not.Null);
                Assert.That(result?.Authors?.Count, Is.EqualTo(_testBook.Authors?.Count));
                Assert.That(result?.Authors?.First()?.ID, Is.EqualTo(_testBook.Authors?.First()?.ID));
                Assert.That(result?.Authors?.First()?.FirstName, Is.EqualTo(_testBook.Authors?.First()?.FirstName));
                Assert.That(result?.Authors?.First()?.LastName, Is.EqualTo(_testBook.Authors?.First()?.LastName));

            });
            Assert.Multiple(() =>
            {
                Assert.That(result?.Genres, Is.Not.Null);
                Assert.That(result?.Genres?.Count, Is.EqualTo(_testBook.Genres?.Count));
                Assert.That(result?.Genres?.First()?.ID, Is.EqualTo(_testBook.Genres?.First()?.ID));
                Assert.That(result?.Genres?.First()?.Name, Is.EqualTo(_testBook.Genres?.First()?.Name));
            });
            Assert.That(result?.Isbn, Is.EqualTo(_testBook.Isbn));
            Assert.That(result?.PublicationYear, Is.EqualTo(_testBook.PublicationYear));
        });
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
        var result = (await _repository.GetAllAsync()).OrderBy(book => book?.ID).ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.First(), Is.Not.Null);
            Assert.That(result.First()?.ID, Is.EqualTo(_testBook.ID));
            Assert.That(result.First()?.Title, Is.EqualTo(_testBook.Title));
            Assert.Multiple(() =>
            {
                Assert.That(result.First()?.Authors, Is.Not.Null);
                Assert.That(result.First()?.Authors?.Count, Is.EqualTo(_testBook.Authors?.Count));
                Assert.That(result.First()?.Authors?.First()?.ID, Is.EqualTo(_testBook.Authors?.First()?.ID));
                Assert.That(result.First()?.Authors?.First()?.FirstName, Is.EqualTo(_testBook.Authors?.First()?.FirstName));
                Assert.That(result.First()?.Authors?.First()?.LastName, Is.EqualTo(_testBook.Authors?.First()?.LastName));

            });
            Assert.Multiple(() =>
            {
                Assert.That(result.First()?.Genres, Is.Not.Null);
                Assert.That(result.First()?.Genres?.Count, Is.EqualTo(_testBook.Genres?.Count));
                Assert.That(result.First()?.Genres?.First()?.ID, Is.EqualTo(_testBook.Genres?.First()?.ID));
                Assert.That(result.First()?.Genres?.First()?.Name, Is.EqualTo(_testBook.Genres?.First()?.Name));
            });
            Assert.That(result.First()?.Isbn, Is.EqualTo(_testBook.Isbn));
            Assert.That(result.First()?.PublicationYear, Is.EqualTo(_testBook.PublicationYear));
            Assert.That(result, Has.Length.GreaterThanOrEqualTo(1));
        });
    }
}