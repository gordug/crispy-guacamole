using System.Runtime.CompilerServices;
using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.Models;

[assembly: InternalsVisibleTo("BookLibrary.BLL.Tests")]

namespace Microsoft.Extensions.DependencyInjection.Mappers;

internal class GenreMapper : IMapper<GenreModel, Genre>
{
    public GenreModel? MapToModel(Genre? genre)
    {
        return genre switch
        {
            null => null,
            _ => new GenreModel(genre.ID, genre.Name)
        };
    }

    public Genre? MapToEntity(GenreModel? genre)
    {
        return genre switch
        {
            null => null,
            _ => new Genre
            {
                ID = genre.Id,
                Name = genre.Name
            }
        };
    }

    public IEnumerable<GenreModel?>? MapToModel(IEnumerable<Genre?>? genres)
    {
        return genres?.Select(MapToModel);
    }

    public IEnumerable<Genre?>? MapToEntity(IEnumerable<GenreModel?>? genres)
    {
        return genres?.Select(MapToEntity);
    }
}
