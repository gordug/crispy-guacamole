namespace BookLibrary.Models;

public class BookModel
{
    public BookModel(string title, List<AuthorModel> authors, List<GenreModel> genres, string isbn, int publicationYear)
    {
        Id = 0;
        Title = title;
        Authors = authors;
        Genres = genres;
        Isbn = isbn;
        PublicationYear = publicationYear;
    }

    public BookModel(int id, string title, List<AuthorModel> authors, List<GenreModel> genres, string isbn,
        int publicationYear)
    {
        Id = id;
        Title = title;
        Authors = authors;
        Genres = genres;
        Isbn = isbn;
        PublicationYear = publicationYear;
    }

    public int Id { get; private set; }
    public string Title { get; set; } = string.Empty;
    public List<AuthorModel> Authors { get; set; } = new();
    public List<GenreModel> Genres { get; set; } = new();
    public string Isbn { get; set; } = string.Empty;
    public int PublicationYear { get; set; }

    public override string ToString()
    {
        return $"{Title} ({PublicationYear})";
    }

    public override bool Equals(object book1)
    {
        return Equals(book1 as BookModel);
    }

    public bool Equals(BookModel book2)
    {
        return Isbn == book2.Isbn;
    }

    public override int GetHashCode()
    {
        return Isbn.GetHashCode();
    }
}