using BookLibrary.BLL.Interfaces;
using BookLibrary.BLL.Services;
using BookLibrary.DAL.Entities;
using BookLibrary.Models;
using Microsoft.Extensions.DependencyInjection.Mappers;

namespace Microsoft.Extensions.DependencyInjection;

public static class BllService
{
    public static void AddBllService(this IServiceCollection services)
    {
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<ILibraryService<BookModel>, BookService>();
        services.AddScoped<ILibraryService<AuthorModel>, AuthorService>();
        services.AddScoped<ILibraryService<GenreModel>, GenreService>();
        services.AddSingleton<IMapper<AuthorModel, Author>, AuthorMapper>();
        services.AddSingleton<IMapper<GenreModel, Genre>, GenreMapper>();
        services.AddSingleton<IMapper<BookModel, Book>, BookMapper>();
    }
}
