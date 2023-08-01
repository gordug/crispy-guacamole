namespace BookLibrary.Models;

public class GenreModel
{
    public GenreModel(string name)
    {
        Id = 0;
        Name = name;
    }

    public GenreModel(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Name}";
    }

    public override bool Equals(object genre1)
    {
        return Equals(genre1 as GenreModel);
    }

    public bool Equals(GenreModel genre2)
    {
        return Name == genre2.Name;
    }
}