using kolos.Models;
using kolos.Repositories;

namespace kolos.Services;

public class BooksService:IBooksService
{
    public readonly IBooksRepository _BooksRepository;

    public BooksService(IBooksRepository booksRepository)
    {
        _BooksRepository = booksRepository;
    }

    public async Task<Books> GetBooksById(int id)
    {
        if (!await _BooksRepository.DoesBookExist(id))
        {
            throw new Exception();
        }
            
        return await _BooksRepository.GetBooksById(id);
    }

    public async Task AddBooks(string title, List<string> genreses)
    {
        if (!await _BooksRepository.DoesBookTitleExist(title))
        {
            throw new Exception();
        }
        await _BooksRepository.AddBooks(title, genreses);
    }
}