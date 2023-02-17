using Application.Books.DTO.Requests;
using Application.Books.DTO.Responses;
using Application.Core.Exceptions;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Books;

public class BookService : IBookService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public BookService(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ListedBookResponseDTO>> GetBooks(BookOrderOptions bookOrder)
    {
        string SelectOrderProperty(Book book)
        {
            return bookOrder switch
            {
                BookOrderOptions.Author => book.Author,
                BookOrderOptions.Title => book.Title,
                _ => throw new BadRequestException("Invalid query parameter")
            };
        }

        var books = await _dataContext.Books.ToListAsync();
        return books.OrderBy(SelectOrderProperty).Select(b => _mapper.Map<ListedBookResponseDTO>(b));
    }

    public async Task<IEnumerable<ListedBookResponseDTO>> GetRecommendedBooks(string? genre)
    {
        if (genre != null && Constants.Genres.Find(g => string.Equals(g, genre)) == null)
            throw new NotFoundException("Genre not found");

        var books = await _dataContext.Books
            .Where(b => b.Reviews.Count > 10)
            .Where(b => genre == null ||
                        string.Equals(b.Genre, genre, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(b => b.Ratings.Count > 0 ? b.Ratings.Average(r => r.Score) : 0)
            .Take(10)
            .Select(b => _mapper.Map<ListedBookResponseDTO>(b))
            .ToListAsync();
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
        var existingBook = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == bookDTO.Id);
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
        var book = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
        if (book == null) throw new NotFoundException("Book with the given ID was not found");
        return book;
    }
}