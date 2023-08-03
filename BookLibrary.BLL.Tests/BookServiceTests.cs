using BookLibrary.BLL.Interfaces;
using BookLibrary.BLL.Services;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using BookLibrary.Models;
using Moq;

namespace BookLibrary.BLL.Tests;

public class BookServiceTests
{
    private readonly Mock<IRepository<Book>> _mockBookRepository;
    private readonly Mock<IMapper<BookModel, Book>> _mockBookMapper;

    public BookServiceTests()
    {
        var mockMappers = new MockMappers.MockMappers();
        _mockBookRepository = new Mock<IRepository<Book>>();
        _mockBookMapper = mockMappers.GetBookMapper();
    }

    private List<AuthorModel> Authors => new() {new AuthorModel("Test", "Author")};

    private List<GenreModel> Genres => new() {new GenreModel("Test Genre")};

    private IEnumerable<BookModel> Books => new List<BookModel>
    {
        new(
            "Test Book",
            Authors,
            Genres,
            "1234567890123",
            2021
           )
    };

    private Book TestBook => new()
    {
        ID = 1,
        Title = "Test Book",
        Authors = Authors.Select(x => new Author
                         {
                             ID = 1,
                             FirstName = x.FirstName,
                             LastName = x.LastName
                         })
                         .ToList(),
        Genres = Genres.Select(x => new Genre
                       {
                           ID = 1,
                           Name = x.Name
                       })
                       .ToList(),
        Isbn = "1234567890123",
        PublicationYear = 2021
    };

    [SetUp]
    public void Setup()
    {
        _mockBookRepository.Reset();
    }

    [Test]
    public void Get_BookExists_ReturnsBook()
    {
        // Arrange
        const int bookId = 1;
        _mockBookRepository.Setup(x => x.GetAsync(bookId)).ReturnsAsync(TestBook);

        var bookService =
            new BookService(_mockBookRepository.Object, _mockBookMapper.Object);

        // Act
        var result = bookService.GetAsync(bookId);

        // Assert
        Assert.That(result.Id, Is.EqualTo(bookId));
    }

    [Test]
    public async Task Get_BookDoesNotExist_ReturnsNull()
    {
        // Arrange
        const int bookId = 2;
        _mockBookRepository.Setup(x => x.GetAsync(bookId)).ReturnsAsync((Book?)null);

        var bookService =
            new BookService(_mockBookRepository.Object, _mockBookMapper.Object);

        // Act
        var result = await bookService.GetAsync(bookId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Get_BookExists_ReturnsBookWithAuthors()
    {
        // Arrange
        const int bookId = 1;
        _mockBookRepository.Setup(x => x.GetAsync(bookId)).ReturnsAsync(TestBook);

        var bookService =
            new BookService(_mockBookRepository.Object, _mockBookMapper.Object);

        // Act
        var result = await bookService.GetAsync(bookId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Authors, Is.Not.Null);
        Assert.That(result?.Authors, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result.Authors.First().FirstName, Is.EqualTo("Test"));
            Assert.That(result.Authors.First().LastName, Is.EqualTo("Author"));
        });
    }

    [Test]
    public async Task Update_BookExists_UpdatesBook()
    {
        // Arrange
        const int bookId = 1;
        var book = Books.First();
        _mockBookRepository.Setup(x => x.GetAsync(bookId)).ReturnsAsync(TestBook);
        _mockBookRepository.Setup(x => x.UpdateAsync(TestBook)).Returns(() => ValueTask.FromResult(TestBook));

        var bookService =
            new BookService(_mockBookRepository.Object, _mockBookMapper.Object);

        // Act
        await bookService.UpdateAsync(book);

        // Assert
        _mockBookRepository.Verify(x => x.UpdateAsync(It.IsAny<Book>()), Times.Once);
    }

    [Test]
    public async Task Delete_Book_CallsDelete()
    {
        // Arrange
        const int bookId = 1;
        var book = Books.First();
        _mockBookRepository.Setup(x => x.GetAsync(bookId)).ReturnsAsync(TestBook);
        _mockBookRepository.Setup(x => x.DeleteAsync(bookId)).Returns(() => ValueTask.FromResult(TestBook));

        var bookService =
            new BookService(_mockBookRepository.Object, _mockBookMapper.Object);

        // Act
        await bookService.DeleteAsync(bookId);

        // Assert
        _mockBookRepository.Verify(x => x.DeleteAsync(bookId), Times.Once);
    }
}
