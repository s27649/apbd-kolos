namespace kolos.Models;

public class Books
{
    public int IdBook { get; set; }
    public string Title { get; set; }
    public Genres Genres { get; set; }
}

public record Genres
{
    public List<string> NameOfGenres { get; set; }
}