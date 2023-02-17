using Application.Books.DTO;
using Domain;

namespace Application.Books;

public interface IBookService
{
    public Task<IEnumerable<ListedBookResponseDTO>> GetBooks(BookOrderOptions bookOrder);
    public Task<IEnumerable<ListedBookResponseDTO>> GetRecommendedBooks(string genre);
    public Task<BookDetailsResponseDTO> GetBookDetails(int bookId);
    public Task DeleteBook(int bookId);
    public Task<IdResponseDTO> SaveBook(SaveBookRequestDTO bookDTO);
    public Task<IdResponseDTO> SaveReview(int bookId, SaveReviewRequestDTO reviewDTO);
    public Task RateBook(int bookId, RateBookRequestDTO rateDTO);
}