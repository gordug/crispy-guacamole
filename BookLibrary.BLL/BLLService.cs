using BookLibrary.BLL.Interfaces;
using BookLibrary.BLL.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class BLLService
{
    public static IServiceCollection AddBLLService(this IServiceCollection services)
    {
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IBookService, BookService>();
        return services;
    }
}