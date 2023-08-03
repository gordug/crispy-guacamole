using System.Runtime.CompilerServices;
using BookLibrary.DAL.Entities;

// InternalsVisibleTo is required for unit testing
[assembly: InternalsVisibleTo("BookLibrary.DAL.Tests")]

namespace BookLibrary.DAL.Repositories;

internal class AuthorRepository : Repository<Author>
{
    public AuthorRepository(LibraryContext context)
        : base(context)
    {
    }
}
