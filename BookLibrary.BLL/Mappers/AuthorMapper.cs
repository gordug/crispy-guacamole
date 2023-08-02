using System.Runtime.CompilerServices;
using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.Models;

[assembly: InternalsVisibleTo("BookLibrary.BLL.Tests")]
namespace Microsoft.Extensions.DependencyInjection.Mappers;


internal class AuthorMapper : IMapper<AuthorModel, Author>
{
    public AuthorModel? MapToModel(Author? author)
    {
        return author switch
        {
            null => null,
            _ => new AuthorModel(author.ID, author.FirstName, author.LastName)
        };
    }

    public Author? MapToEntity(AuthorModel? author)
    {
        return author switch
        {
            null => null,
            _ => new Author
            {
                FirstName = author.FirstName,
                LastName = author.LastName
            }
        };
    }

    public IEnumerable<AuthorModel?>? MapToModel(IEnumerable<Author?>? authors)
    {
        return authors?.Select(MapToModel);
    }

    public IEnumerable<Author?>? MapToEntity(IEnumerable<AuthorModel?>? authors)
    {
        return authors?.Select(MapToEntity);
    }
}
