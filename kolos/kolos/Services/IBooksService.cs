using kolos.Models;

namespace kolos.Services;

public interface IBooksService
{
    public Task<Books> GetBooksById(int id);
    public Task AddBooks(string title,List<string>genreses);
}