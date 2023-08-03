using BookLibrary.Models;

namespace BookLibrary.BLL.Interfaces;

public interface IBookService : ISearchable<BookModel>
{
    public Task<IEnumerable<BookModel?>?> GetByAuthorAsync(int authorId);
    public Task<IEnumerable<BookModel?>?> GetByGenreAsync(int genreId);
    public Task<IEnumerable<BookModel?>?> GetByPublishedYearAsync(int year);
}
