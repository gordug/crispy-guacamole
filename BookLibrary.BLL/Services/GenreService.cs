using System.Runtime.CompilerServices;
using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using BookLibrary.Models;

[assembly: InternalsVisibleTo("BookLibrary.BLL.Tests")]

namespace BookLibrary.BLL.Services;

public class GenreService : LibraryService<GenreModel, Genre>, IGenreService
{
    public GenreService(IRepository<Genre> genreRepository) : base(genreRepository)
    {
    }

    public async Task<IEnumerable<GenreModel?>?> SearchAsync(string query)
    {
        var genres = await _repository.GetAllAsync();
        return MapToModel(genres.Where(genre => genre is not null && genre.Name.Contains(query)));
    }

    internal override GenreModel? MapToModel(Genre? genre)
    {
        return genre switch
        {
            null => null,
            _ => new GenreModel(genre.ID, genre.Name)
        };
    }

    internal override Genre? MapToEntity(GenreModel? genre)
    {
        return genre switch
        {
            null => null,
            _ => new Genre { ID = genre.Id, Name = genre.Name }
        };
    }

    internal override IEnumerable<GenreModel?>? MapToModel(IEnumerable<Genre?>? genres)
    {
        return genres?.Select(MapToModel);
    }

    internal override IEnumerable<Genre?>? MapToEntity(IEnumerable<GenreModel?>? genres)
    {
        return genres?.Select(MapToEntity);
    }
}