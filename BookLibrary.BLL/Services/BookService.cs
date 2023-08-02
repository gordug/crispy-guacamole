using System.Runtime.CompilerServices;
using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using BookLibrary.Models;

[assembly: InternalsVisibleTo("BookLibrary.BLL.Tests")]

namespace BookLibrary.BLL.Services;

internal sealed class BookService : LibraryService<BookModel, Book>, IBookService
{
    public BookService(IRepository<Book> bookRepository, IMapper<BookModel, Book> mapper) : base(bookRepository, mapper)
    {
    }


    public async Task<IEnumerable<BookModel?>?> SearchAsync(string query)
    {
        var books = await Repository.GetAllAsync();
        return Mapper.MapToModel(books.Where(book =>
        {
            return book is { } &&
                   (book.Title.Contains(query) ||
                    book.Authors.Any(author => author.FirstName.Contains(query) || author.LastName.Contains(query)) ||
                    book.Genres.Any(genre => genre.Name.Contains(query)));
        }));
    }

    public Task<IEnumerable<BookModel?>?> GetByAuthorAsync(int authorId)
    {
        var books = Repository.GetAllAsync().Result;
        return Task.FromResult(Mapper.MapToModel(books.Where(book =>
                                                                 book is { } && book.Authors.Any(author => author.ID == authorId))));
    }

    public Task<IEnumerable<BookModel?>?> GetByGenreAsync(int genreId)
    {
        var books = Repository.GetAllAsync().Result;
        return Task.FromResult(Mapper.MapToModel(books.Where(book =>
                                                                 book is { } && book.Genres.Any(genre => genre.ID == genreId))));
    }

    public Task<IEnumerable<BookModel?>?> GetByPublishedYearAsync(int year)
    {
        var books = Repository.GetAllAsync().Result;
        return Task.FromResult(Mapper.MapToModel(books.Where(book => book is { } && book.PublicationYear == year)));
    }

    
}