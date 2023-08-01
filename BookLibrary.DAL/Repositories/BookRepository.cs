using System.Runtime.CompilerServices;
using BookLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

// InternalsVisibleTo is required for unit testing
[assembly: InternalsVisibleTo("BookLibrary.DAL.Tests")]

namespace BookLibrary.DAL.Repositories;

internal class BookRepository : Repository<Book>
{

    public BookRepository(LibraryContext context) : base(context) { }
}