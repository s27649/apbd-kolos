using kolos.Models;

namespace kolos.Repositories;

public interface IBooksRepository
{
    public Task<Books> GetBooksById(int id);
    public Task AddBooks(string title,List<string>genreses);
    
    public Task<bool> DoesBookTitleExist(string title);
    public Task<bool> DoesBookExist(int id);
}