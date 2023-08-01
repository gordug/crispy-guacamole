using System.Runtime.CompilerServices;
using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using BookLibrary.Models;

[assembly: InternalsVisibleTo("BookLibrary.BLL.Tests")]

namespace BookLibrary.BLL.Services;

internal sealed class AuthorService : LibraryService<AuthorModel, Author>, IAuthorService
{
    public AuthorService(IRepository<Author> authorRepository) : base(authorRepository)
    {
    }

    public async Task<IEnumerable<AuthorModel?>?> SearchAsync(string query)
    {
        var authors = await _repository.GetAllAsync();
        return MapToModel(authors.Where(author =>
            author is not null && (author.FirstName.Contains(query) || author.LastName.Contains(query))));
    }

    internal override AuthorModel? MapToModel(Author? author)
    {
        return author switch
        {
            null => null,
            _ => new AuthorModel(author.ID, author.FirstName, author.LastName)
        };
    }

    internal override Author? MapToEntity(AuthorModel? author)
    {
        return author switch
        {
            null => null,
            _ => new Author { FirstName = author.FirstName, LastName = author.LastName }
        };
    }

    internal override IEnumerable<AuthorModel?>? MapToModel(IEnumerable<Author?>? authors)
    {
        return authors?.Select(MapToModel);
    }

    internal override IEnumerable<Author?>? MapToEntity(IEnumerable<AuthorModel?>? authors)
    {
        return authors?.Select(MapToEntity);
    }
}