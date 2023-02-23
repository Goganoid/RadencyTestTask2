using Application.Books.DTO.Requests;
using Application.Books.DTO.Responses;
using Application.Core.Exceptions;
using AutoMapper;
using Dapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Books;

public class BookService : IBookService
{
    private readonly DataContext _dataContext;
    private readonly DapperContext _dapperContext;
    private readonly IMapper _mapper;

    public BookService(DataContext dataContext, IMapper mapper, DapperContext dapperContext)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _dapperContext = dapperContext;
    }

    public async Task<IEnumerable<ListedBookResponseDTO>> GetBooks(BookOrderOptions bookOrder)
    {
        string SelectOrderProperty()
        {
            return bookOrder switch
            {
                BookOrderOptions.Author => "Author",
                BookOrderOptions.Title => "Title",
                _ => throw new BadRequestException("Invalid query parameter")
            };
        }

        var bookDictionary = new Dictionary<int, Book>();
        using var connection = _dapperContext.CreateConnection();
        var booksQuery = await connection.QueryAsync<Book, Rating, Book>(
            """ SELECT * FROM "Books" as b LEFT JOIN "Ratings" as r ON b."BookId"=r."BookId"  ORDER BY @order """,
            ((book, rating) =>
            {
                if (!bookDictionary.TryGetValue(book.BookId, out var bookEntry))
                {
                    bookEntry = book;
                    bookEntry.Ratings = new();
                    bookDictionary.Add(book.BookId, bookEntry);
                }

                bookEntry.Ratings.Add(rating);
                return bookEntry;
            }), new {order = SelectOrderProperty()}, splitOn: """ RatingId """);
        var books = booksQuery.Distinct().ToList();
        return books.Select(b => _mapper.Map<ListedBookResponseDTO>(b));
    }

    public async Task<IEnumerable<ListedBookResponseDTO>> GetRecommendedBooks(string? genre)
    {
        if (genre != null && Constants.Genres.Find(g => string.Equals(g, genre)) == null)
            throw new NotFoundException("Genre not found");
        var bookDictionary = new Dictionary<int, Book>();
        using var connection = _dapperContext.CreateConnection();
        var builder = new SqlBuilder();

        if (!string.IsNullOrEmpty(genre)) builder.Where($""" b."Genre"= '{genre}' """);
        var selector = builder.AddTemplate(""" 
               SELECT  *  FROM "Books" as b 
    LEFT JOIN "Ratings" as r ON b."BookId"=r."BookId"
    RIGHT JOIN (SELECT b."BookId",avg(coalesce(r."Score",0)) as score 
                FROM "Books" as b 
                    LEFT JOIN "Ratings" as r ON b."BookId"=r."BookId"
               /**where**/
                GROUP BY b."BookId" 
                HAVING count(r."Score")>10
                ORDER BY score DESC
                LIMIT 10) averages
        on b."BookId"=averages."BookId"
    ORDER BY averages.score DESC 
 """);
        var booksQuery = await connection.QueryAsync<Book, Rating, Book>(
            selector.RawSql,
            ((book, rating) =>
            {
                if (!bookDictionary.TryGetValue(book.BookId, out var bookEntry))
                {
                    bookEntry = book;
                    bookEntry.Ratings = new();
                    bookDictionary.Add(book.BookId, bookEntry);
                }

                bookEntry.Ratings.Add(rating);
                return bookEntry;
            }), splitOn: """ RatingId """);
        var books = booksQuery.Distinct().ToList().Select(b => _mapper.Map<ListedBookResponseDTO>(b));
        return books;
    }

    public async Task<BookDetailsResponseDTO> GetBookDetails(int bookId)
    {
        var book = await GetBook(bookId);
        return _mapper.Map<BookDetailsResponseDTO>(book);
    }

    public async Task DeleteBook(int bookId)
    {
        var book = await GetBook(bookId);
        _dataContext.Books.Remove(book);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<IdResponseDTO> SaveBook(SaveBookRequestDTO bookDTO)
    {
        var book = _mapper.Map<Book>(bookDTO);
        var existingBook = await _dataContext.Books.FirstOrDefaultAsync(b => b.BookId == bookDTO.BookId);
        if (existingBook != null)
        {
            existingBook.Author = book.Author;
            existingBook.Content = book.Content;
            existingBook.Cover = book.Cover;
            existingBook.Genre = book.Genre;
            existingBook.Title = book.Title;
            _dataContext.Books.Update(existingBook);
        }
        else
        {
            await _dataContext.Books.AddAsync(book);
        }

        await _dataContext.SaveChangesAsync();
        return _mapper.Map<IdResponseDTO>(book);
    }

    public async Task<IdResponseDTO> SaveReview(int bookId, SaveReviewRequestDTO reviewDTO)
    {
        var book = await GetBook(bookId);
        var review = _mapper.Map<Review>(reviewDTO);
        book.Reviews.Add(review);
        await _dataContext.SaveChangesAsync();
        return _mapper.Map<IdResponseDTO>(review);
    }

    public async Task RateBook(int bookId, RateBookRequestDTO rateDTO)
    {
        var book = await GetBook(bookId);
        var rating = _mapper.Map<Rating>(rateDTO);
        book.Ratings.Add(rating);
        await _dataContext.SaveChangesAsync();
    }

    private async Task<Book> GetBook(int bookId)
    {
        using var connection = _dapperContext.CreateConnection();
        var bookDictionary = new Dictionary<int, Book>();
        var bookQuery = await connection.QueryAsync<Book, Rating, Book>(""" 
SELECT  *  FROM "Books" as b 
    LEFT JOIN "Ratings" as ratings ON b."BookId"=ratings."BookId"
    WHERE b."BookId"=@bookId
""", (book, rating) =>
        {
            if (!bookDictionary.TryGetValue(book.BookId, out var bookEntry))
            {
                bookEntry = book;
                bookEntry.Ratings = new();
                bookDictionary.Add(book.BookId, bookEntry);
            }

            bookEntry.Ratings.Add(rating);
            return bookEntry;
        }, new {bookId}, splitOn: "RatingId");
        var book = bookQuery.FirstOrDefault();
        if (book == null) throw new NotFoundException("Book with the given ID was not found");

        var reviewsQuery = await connection.QueryAsync<Review>(
            """
    SELECT * FROM "Reviews" as r
    WHERE  r."BookId"=@bookId
""", new {bookId});
        book.Reviews = reviewsQuery.ToList();


        return book;
    }
}