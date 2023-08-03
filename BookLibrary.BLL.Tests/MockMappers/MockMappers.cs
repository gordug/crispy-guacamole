using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.Models;
using Moq;

namespace BookLibrary.BLL.Tests.MockMappers;

public class MockMappers
{
    private readonly Mock<IMapper<AuthorModel, Author>> _authorMapperMock;
    private readonly Mock<IMapper<GenreModel, Genre>> _genreMapperMock;
    private readonly Mock<IMapper<BookModel, Book>> _bookMapperMock;

    public MockMappers()
    {
        _authorMapperMock = new Mock<IMapper<AuthorModel, Author>>();
        _genreMapperMock = new Mock<IMapper<GenreModel, Genre>>();
        _bookMapperMock = new Mock<IMapper<BookModel, Book>>();
        _authorMapperMock.Setup(mapper => mapper.MapToModel(It.IsAny<Author?>()))
                         .Returns((Author? author) => author is null ? null : new AuthorModel(author.ID, author.FirstName, author.LastName));
        _authorMapperMock.Setup(mapper => mapper.MapToEntity(It.IsAny<AuthorModel>()))
                         .Returns((AuthorModel author) => new Author
                         {
                             ID = author.Id,
                             FirstName = author.FirstName,
                             LastName = author.LastName
                         });
        _authorMapperMock.Setup(mapper => mapper.MapToEntity(It.IsAny<IEnumerable<AuthorModel?>?>()))
                         .Returns((IEnumerable<AuthorModel?>? authors) => authors?.Select(author => author is null
                                                                                              ? null
                                                                                              : new Author
                                                                                              {
                                                                                                  ID = author.Id,
                                                                                                  FirstName = author.FirstName,
                                                                                                  LastName = author.LastName
                                                                                              }));
        _authorMapperMock.Setup(mapper => mapper.MapToModel(It.IsAny<IEnumerable<Author?>?>()))
                         .Returns((IEnumerable<Author?>? authors) => authors?.Select(author => author is null ? null : new AuthorModel(author.ID, author.FirstName, author.LastName)));
        _genreMapperMock.Setup(mapper => mapper.MapToModel(It.IsAny<Genre?>()))
                        .Returns((Genre? genre) => genre is null ? null : new GenreModel(genre.ID, genre.Name));
        _genreMapperMock.Setup(mapper => mapper.MapToModel(It.IsAny<IEnumerable<Genre?>?>()))
                        .Returns((IEnumerable<Genre?>? genres) => genres?.Select(genre => genre is null ? null : new GenreModel(genre.ID, genre.Name)).ToList());
        _genreMapperMock.Setup(mapper => mapper.MapToEntity(It.IsAny<GenreModel?>()))
                        .Returns((GenreModel? genre) => genre is null
                                     ? null
                                     : new Genre
                                     {
                                         ID = genre.Id,
                                         Name = genre.Name
                                     });
        _genreMapperMock.Setup(mapper => mapper.MapToEntity(It.IsAny<IEnumerable<GenreModel?>?>()))
                        .Returns((IEnumerable<GenreModel?>? genres) => genres?.Select(genre => genre is null
                                                                                          ? null
                                                                                          : new Genre
                                                                                          {
                                                                                              ID = genre.Id,
                                                                                              Name = genre.Name
                                                                                          })
                                                                             .ToList());
        _bookMapperMock.Setup(mapper => mapper.MapToModel(It.IsAny<Book?>()))
                       .Returns((Book? book) => book is null
                                    ? null
                                    : new BookModel(
                                                    book.ID,
                                                    book.Title,
                                                    _authorMapperMock.Object.MapToModel(book.Authors)?.ToList(),
                                                    _genreMapperMock.Object.MapToModel(book.Genres)?.ToList(),
                                                    book.Isbn,
                                                    book.PublicationYear));
        _bookMapperMock.Setup(mapper => mapper.MapToModel(It.IsAny<IEnumerable<Book?>?>()))
                       .Returns((IEnumerable<Book?>? books) => books?.Select(book => book is null
                                                                                 ? null
                                                                                 : new BookModel(
                                                                                                 book.ID,
                                                                                                 book.Title,
                                                                                                 _authorMapperMock.Object.MapToModel(book.Authors)?.ToList(),
                                                                                                 _genreMapperMock.Object.MapToModel(book.Genres)?.ToList(),
                                                                                                 book.Isbn,
                                                                                                 book.PublicationYear))
                                                                    .ToList());
        _bookMapperMock.Setup(mapper => mapper.MapToEntity(It.IsAny<BookModel?>()))
                       .Returns((BookModel? book) => book is null
                                    ? null
                                    : new Book
                                    {
                                        ID = book.Id,
                                        Title = book.Title,
                                        Authors = _authorMapperMock.Object.MapToEntity(book.Authors)?.ToList(),
                                        Genres = _genreMapperMock.Object.MapToEntity(book.Genres)?.ToList(),
                                        Isbn = book.Isbn,
                                        PublicationYear = book.PublicationYear
                                    });
        _bookMapperMock.Setup(mapper => mapper.MapToEntity(It.IsAny<IEnumerable<BookModel?>?>()))
                       .Returns((IEnumerable<BookModel?>? books) => books?.Select(book => book is null
                                                                                      ? null
                                                                                      : new Book
                                                                                      {
                                                                                          ID = book.Id,
                                                                                          Title = book.Title,
                                                                                          Authors = _authorMapperMock.Object.MapToEntity(book.Authors)?.ToList(),
                                                                                          Genres = _genreMapperMock.Object.MapToEntity(book.Genres)?.ToList(),
                                                                                          Isbn = book.Isbn,
                                                                                          PublicationYear = book.PublicationYear
                                                                                      })
                                                                         .ToList());
    }

    public Mock<IMapper<AuthorModel, Author>> GetAuthorMapper()
    {
        return _authorMapperMock;
    }

    public Mock<IMapper<GenreModel, Genre>> GetGenreMapper()
    {
        return _genreMapperMock;
    }

    public Mock<IMapper<BookModel, Book>> GetBookMapper()
    {
        return _bookMapperMock;
    }
}
