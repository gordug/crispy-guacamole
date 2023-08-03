using BookLibrary.DAL;
using BookLibrary.DAL.Entities;
using BookLibrary.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// Added To DI namespace for ease of reference
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DALServices
{
    public static void AddContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<LibraryContext>(options =>
                                                  options.UseSqlServer(configuration.GetConnectionString("BookLibraryContext")));
        services.AddScoped<IRepository<Book>, BookRepository>();
        services.AddScoped<IRepository<Author>, AuthorRepository>();
        services.AddScoped<IRepository<Genre>, GenreRepository>();
    }
}
