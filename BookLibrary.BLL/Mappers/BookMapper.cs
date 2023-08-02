using System.Runtime.CompilerServices;
using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.Models;

[assembly: InternalsVisibleTo("BookLibrary.BLL.Tests")]
namespace Microsoft.Extensions.DependencyInjection.Mappers;

internal class BookMapper : IMapper<BookModel, Book>
{
    private readonly IMapper<AuthorModel, Author> _authorMapper;
    private readonly IMapper<GenreModel, Genre> _genreMapper;

    public BookMapper(IMapper<AuthorModel, Author> authorMapper,
                      IMapper<GenreModel, Genre> genreMapper)
    {
        _authorMapper = authorMapper;
        _genreMapper = genreMapper;
    }

    public IEnumerable<BookModel?>? MapToModel(IEnumerable<Book?>? books)
    {
        return books?.Select(MapToModel);
    }

    public BookModel? MapToModel(Book? book)
    {
        return book switch
        {
            null => null,
            _ => new BookModel(book.ID,
                               book.Title,
                               _authorMapper.MapToModel(book.Authors)?.ToList() ?? null,
                               _genreMapper.MapToModel(book.Genres)?.ToList() ?? null,
                               book.Isbn,
                               book.PublicationYear)
        };
    }

    public Book? MapToEntity(BookModel? bookModel)
    {
        return bookModel switch
        {
            null => null,
            _ => new Book
            {
                ID = bookModel.Id,
                Title = bookModel.Title,
                Authors = _authorMapper.MapToEntity(bookModel.Authors)?.ToList(),
                Genres = _genreMapper.MapToEntity(bookModel.Genres)?.ToList(),
                Isbn = bookModel.Isbn,
                PublicationYear = bookModel.PublicationYear
            }
        };
    }

    public IEnumerable<Book?>? MapToEntity(IEnumerable<BookModel?>? books)
    {
        return books?.Select(MapToEntity);
    }
}
