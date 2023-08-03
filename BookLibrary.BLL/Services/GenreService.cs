using System.Runtime.CompilerServices;
using BookLibrary.BLL.Interfaces;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using BookLibrary.Models;

[assembly: InternalsVisibleTo("BookLibrary.BLL.Tests")]

namespace BookLibrary.BLL.Services;

public sealed class GenreService : LibraryService<GenreModel, Genre>, IGenreService
{
    public GenreService(
        IRepository<Genre> genreRepository,
        IMapper<GenreModel, Genre> mapper)
        : base(genreRepository, mapper)
    {
    }

    public async Task<IEnumerable<GenreModel?>?> SearchAsync(string query)
    {
        var genres = await Repository.GetAllAsync();
        return Mapper.MapToModel(genres.Where(genre => genre is { } && genre.Name.Contains(query)));
    }
}
