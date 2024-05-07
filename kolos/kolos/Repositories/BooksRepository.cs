using kolos.Models;
using Microsoft.Data.SqlClient;

namespace kolos.Repositories;

public class BooksRepository: IBooksRepository
{
    private IConfiguration _configuration;

    public BooksRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> DoesBookExist(int id)
    {
        var query = "SELECT 1 FROM books WHERE PK = @IdBooks";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdBooks", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();
        
        return res is not null;
    }
    
    public async Task<bool> DoesBookTitleExist(string title)
    {
        var query = "SELECT 1 FROM books WHERE title = @Title";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@Title", title);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();
        
        return res is not null;
    }

    public async Task<Books> GetBooksById(int id)
    {
        var query = @"SELECT b.PK AS id,b.title AS title, g.name AS genres from books b
                        JOIN books_genres bg ON b.PK=bg.FK_book
                        JOIN genres g ON bg.FK_genre=g.PK
                        WHERE b.PK=@IdBooks;";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdBooks", id);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();

        await reader.ReadAsync();

        if (!reader.HasRows) throw new Exception();

        List<string> genres = new List<string>();
        var booksIDOrdinal = reader.GetOrdinal("id");
        var titleOrdinal = reader.GetOrdinal("title");
        var genresOrdinal = reader.GetOrdinal("genres");
        while (reader.Read())
        {
            genres.Add(reader.GetString(genresOrdinal)); 
        }
        var books = new Books()
        {
            IdBook = reader.GetInt32(booksIDOrdinal),
            Title = reader.GetString(titleOrdinal),
            Genres = new Genres()
            {
                NameOfGenres = genres
            }
        };
        
        return books;
    }

    public async Task AddBooks(string title, List<String> genrese)
    {
        var query = @"INSERT INTO books VALUES (@Title,@IdBooks, @IdGenres); 
                    SELECT @@IDENTITY ";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.Parameters.AddWithValue("@Title", title);
        await connection.OpenAsync();
        var id = await connection.BeginTransactionAsync();
        foreach (var idGenrese in genrese)
        {
            command.CommandText =query;
            command.Parameters.AddWithValue("@IdBooks", id);
            command.Parameters.AddWithValue("@IdGenres", idGenrese);
            await command.ExecuteNonQueryAsync();
        }
    }
    }
    