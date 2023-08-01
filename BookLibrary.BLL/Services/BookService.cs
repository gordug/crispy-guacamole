using System.Runtime.CompilerServices;
using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using BookLibrary.Models;

[assembly: InternalsVisibleTo("BookLibrary.BLL.Tests")]

namespace BookLibrary.BLL.Services;

internal sealed class BookService : LibraryService<BookModel, Book>, IBookService
{
    private readonly ILibraryService<Author> _authorService;
    private readonly ILibraryService<Genre> _genreService;

    public BookService(IRepository<Book> bookRepository, ILibraryService<Genre> genreService,
        ILibraryService<Author> authorService) : base(bookRepository)
    {
        _genreService = genreService;
        _authorService = authorService;
    }


    public async Task<IEnumerable<BookModel?>?> SearchAsync(string query)
    {
        var books = await _repository.GetAllAsync();
        return MapToModel(books.Where(book =>
        {
            return book is not null &&
                   (book.Title.Contains(query) ||
                    book.Authors.Any(author => author.FirstName.Contains(query) || author.LastName.Contains(query)) ||
                    book.Genres.Any(genre => genre.Name.Contains(query)));
        }));
    }

    public Task<IEnumerable<BookModel?>?> GetByAuthorAsync(int authorId)
    {
        var books = _repository.GetAllAsync().Result;
        return Task.FromResult(MapToModel(books.Where(book =>
            book is not null && book.Authors.Any(author => author.ID == authorId))));
    }

    public Task<IEnumerable<BookModel?>?> GetByGenreAsync(int genreId)
    {
        var books = _repository.GetAllAsync().Result;
        return Task.FromResult(MapToModel(books.Where(book =>
            book is not null && book.Genres.Any(genre => genre.ID == genreId))));
    }

    public Task<IEnumerable<BookModel?>?> GetByPublishedYearAsync(int year)
    {
        var books = _repository.GetAllAsync().Result;
        return Task.FromResult(MapToModel(books.Where(book => book is not null && book.PublicationYear == year)));
    }

    internal override IEnumerable<BookModel?>? MapToModel(IEnumerable<Book?>? books)
    {
        return books?.Select(MapToModel);
    }

    internal override BookModel? MapToModel(Book? book)
    {
        return book switch
        {
            null => null,
            _ => new BookModel(book.ID, book.Title,
                (from author in book.Authors select new AuthorModel(author.ID, author.FirstName, author.LastName))
                .ToList(), (from genre in book.Genres select new GenreModel(genre.ID, genre.Name)).ToList(), book.Isbn,
                book.PublicationYear)
        };
    }

    internal override Book? MapToEntity(BookModel? bookModel)
    {
        return bookModel switch
        {
            null => null,
            _ => new Book
            {
                ID = bookModel.Id,
                Title = bookModel.Title,
                Authors =
                    (from author in bookModel.Authors select _authorService.GetAsync(author.Id).Result).ToList(),
                Genres =
                    (from genre in bookModel.Genres select _genreService.GetAsync(genre.Id).Result).ToList(),
                Isbn = bookModel.Isbn,
                PublicationYear = bookModel.PublicationYear
            }
        };
    }

    internal override IEnumerable<Book?>? MapToEntity(IEnumerable<BookModel?>? books)
    {
        return books?.Select(MapToEntity);
    }
}