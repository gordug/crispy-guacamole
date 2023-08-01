using BookLibrary.BLL.Interfaces;
using BookLibrary.BLL.Services;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using BookLibrary.Models;
using Moq;

namespace BookLibrary.BLL.Tests;

public class BookServiceTests
{
    private readonly Mock<ILibraryService<Author>> _mockAuthorService;
    private readonly Mock<IRepository<Book>> _mockBookRepository;
    private readonly Mock<ILibraryService<Genre>> _mockGenreService;

    public BookServiceTests()
    {
        _mockBookRepository = new Mock<IRepository<Book>>();
        _mockAuthorService = new Mock<ILibraryService<Author>>();
        _mockGenreService = new Mock<ILibraryService<Genre>>();
    }

    private List<AuthorModel> Authors =>
        new() { new AuthorModel("Test", "Author") };

    private List<GenreModel> Genres =>
        new() { new GenreModel("Test Genre") };

    private IEnumerable<BookModel> Books =>
        new List<BookModel>
        {
            new(
                "Test Book",
                Authors,
                Genres,
                "1234567890123",
                2021
            )
        };

    private Book TestBook =>
        new()
        {
            Id = 1,
            Title = "Test Book",
            Authors = Authors.Select(x => new Author { ID = 1, FirstName = x.FirstName, LastName = x.LastName })
                .ToList(),
            Genres = Genres.Select(x => new Genre { ID = 1, Name = x.Name }).ToList(),
            Isbn = "1234567890123",
            PublicationYear = 2021
        };


    [SetUp]
    public void Setup()
    {
        _mockBookRepository.Reset();
        _mockAuthorService.Reset();
        _mockGenreService.Reset();
    }

    [Test]
    public void Get_BookExists_ReturnsBook()
    {
        // Arrange
        const int bookId = 1;
        _mockBookRepository.Setup(x => x.GetAsync(bookId)).ReturnsAsync(TestBook);

        var bookService =
            new BookService(_mockBookRepository.Object, _mockGenreService.Object, _mockAuthorService.Object);

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
            new BookService(_mockBookRepository.Object, _mockGenreService.Object, _mockAuthorService.Object);

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
            new BookService(_mockBookRepository.Object, _mockGenreService.Object, _mockAuthorService.Object);

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
        _mockBookRepository.Setup(x => x.UpdateAsync(TestBook)).Returns(() => Task.FromResult(TestBook));

        var bookService =
            new BookService(_mockBookRepository.Object, _mockGenreService.Object, _mockAuthorService.Object);

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
        _mockBookRepository.Setup(x => x.DeleteAsync(bookId)).Returns(() => Task.FromResult(TestBook));

        var bookService =
            new BookService(_mockBookRepository.Object, _mockGenreService.Object, _mockAuthorService.Object);

        // Act
        await bookService.DeleteAsync(bookId);

        // Assert
        _mockBookRepository.Verify(x => x.DeleteAsync(bookId), Times.Once);
    }

    [Test]
    public void ConvertToBookModel_ReturnsBookModel()
    {
        // Arrange
        var bookService =
            new BookService(_mockBookRepository.Object, _mockGenreService.Object, _mockAuthorService.Object);

        // Act
        var result = bookService.MapToModel(TestBook);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Title, Is.EqualTo(TestBook.Title));
            Assert.That(result.Authors, Is.Not.Null);
            Assert.That(result.Authors, Has.Count.EqualTo(1));
            Assert.That(result.Authors.First().FirstName, Is.EqualTo(TestBook.Authors.First().FirstName));
            Assert.That(result.Authors.First().LastName, Is.EqualTo(TestBook.Authors.First().LastName));
            Assert.That(result.Genres, Is.Not.Null);
            Assert.That(result.Genres, Has.Count.EqualTo(1));
            Assert.That(result.Genres.First().Name, Is.EqualTo(TestBook.Genres.First().Name));
            Assert.That(result.Isbn, Is.EqualTo(TestBook.Isbn));
            Assert.That(result.PublicationYear, Is.EqualTo(TestBook.PublicationYear));
        });
    }

    [Test]
    public void ConvertToBook_ReturnsBook()
    {
        // Arrange
        _mockAuthorService.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(TestBook.Authors.First());
        _mockGenreService.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(TestBook.Genres.First());
        var bookService =
            new BookService(_mockBookRepository.Object, _mockGenreService.Object, _mockAuthorService.Object);

        // Act
        var result = bookService.MapToEntity(Books.First());
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.Title, Is.EqualTo(Books.First().Title));
            Assert.That(result.Authors, Is.Not.Null);
        });
        Assert.That(result.Authors, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result.Authors.First().FirstName, Is.EqualTo(Books.First().Authors.First().FirstName));
            Assert.That(result.Authors.First().LastName, Is.EqualTo(Books.First().Authors.First().LastName));
            Assert.That(result.Genres, Is.Not.Null);
        });
        Assert.That(result.Genres, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result.Genres.First().Name, Is.EqualTo(Books.First().Genres.First().Name));
            Assert.That(result.Isbn, Is.EqualTo(Books.First().Isbn));
            Assert.That(result.PublicationYear, Is.EqualTo(Books.First().PublicationYear));
        });
    }
}