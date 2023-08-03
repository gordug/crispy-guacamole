namespace BookLibrary.Models;

public class AuthorModel
{
    public AuthorModel(
        string firstName,
        string lastName)
    {
        Id = 0;
        FirstName = firstName;
        LastName = lastName;
    }

    public AuthorModel(
        int id,
        string firstName,
        string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public AuthorModel()
    {
    }

    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }

    public override bool Equals(object author1)
    {
        return Equals(author1 as AuthorModel);
    }

    public bool Equals(AuthorModel author2)
    {
        return FirstName == author2.FirstName && LastName == author2.LastName;
    }
}
