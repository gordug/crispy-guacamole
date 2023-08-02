using System.Runtime.CompilerServices;
using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using BookLibrary.Models;

[assembly: InternalsVisibleTo("BookLibrary.BLL.Tests")]

namespace BookLibrary.BLL.Services;

internal sealed class AuthorService : LibraryService<AuthorModel, Author>, IAuthorService
{
    public AuthorService(IRepository<Author> authorRepository, IMapper<AuthorModel, Author> mapper) : base(authorRepository, mapper)
    {
    }

    public async Task<IEnumerable<AuthorModel?>?> SearchAsync(string query)
    {
        var authors = await Repository.GetAllAsync();
        return Mapper.MapToModel(authors.Where(author =>
            author is { } && (author.FirstName.Contains(query) || author.LastName.Contains(query))));
    }

    
}