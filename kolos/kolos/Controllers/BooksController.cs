using kolos.Services;
using Microsoft.AspNetCore.Mvc;

namespace kolos.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BooksController:ControllerBase
{
    public readonly IBooksService _BooksService;

    public BooksController(IBooksService booksService)
    {
        _BooksService = booksService;
    }

    [HttpGet]
    [Route("/api/books/{id}/genres")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var books = await _BooksService.GetBooksById(id);
        return Ok(books);
    }

    [HttpPost]
    [Route("/api/books")]
    public async Task<IActionResult> AddBook(string title, List<string> genreses)
    {
        await _BooksService.AddBooks(title, genreses);
        return StatusCode(StatusCodes.Status201Created);
    }
}