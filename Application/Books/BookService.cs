using Application.Books.DTO;
using Application.Core.Exceptions;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using static Domain.BookExtensions;
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
        Func<Book, string> selectOrderProperty = book => bookOrder switch
        {
            BookOrderOptions.Author => book.Author,
            BookOrderOptions.Title => book.Title,
            _=> throw new BadRequestException("Invalid query parameter")
        };
        var books = await _dataContext.Books.ToListAsync();
        return books.OrderBy(b =>selectOrderProperty(b)).Select(b=>_mapper.Map<ListedBookResponseDTO>(b));
    }

    public async Task<IEnumerable<ListedBookResponseDTO>> GetRecommendedBooks(string genre)
    {
        if (Constants.Genres.Find(g => string.Equals(g, genre)) == null)
        {
            throw new NotFoundException("Genre not found");
        }
        var books = await _dataContext.Books
            .Where(b => b.Reviews.Count > 10)
            .Where(b=>string.Equals(b.Genre,genre,StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(b => b.Ratings.Count>0 ? b.Ratings.Average(r=>r.Score) : 0)
            .Take(10)
            .Select(b=>_mapper.Map<ListedBookResponseDTO>(b))
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
        if (bookDTO.Id != null &&
            await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == bookDTO.Id) != null)
            throw new ConflictException("Book with given ID already exists");
        var book = _mapper.Map<Book>(bookDTO);
        await _dataContext.Books.AddAsync(book);
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
        if (book == null)
        {
            throw new NotFoundException("Book with given ID was not found");
        }

        return book;
    }
}