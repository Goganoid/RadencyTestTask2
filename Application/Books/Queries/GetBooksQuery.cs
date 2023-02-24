using Application.Books.Commands.Books;
using Application.Books.Commands.Core.Exceptions;
using AutoMapper;
using Dapper;
using Domain;
using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Books.Queries;


public record GetBooksQuery(BookOrderOptions BookOrder) : IRequest<IEnumerable<ListedBookResponseDTO>>, IRequest<BookDetailsResponseDTO>;

public class GetBooks : IRequestHandler<GetBooksQuery,IEnumerable<ListedBookResponseDTO>>
{
    private readonly DapperContext _dapperContext;
    private readonly IMapper _mapper;

    public GetBooks(DapperContext dapperContext, IMapper mapper)
    {
        _dapperContext = dapperContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ListedBookResponseDTO>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        string SelectOrderProperty()
        {
            return request.BookOrder switch
            {
                BookOrderOptions.Author => "Author",
                BookOrderOptions.Title => "Title",
                _ => throw new BadRequestException("Invalid query parameter")
            };
        }

        var bookDictionary = new Dictionary<int, Book>();
        using var connection = _dapperContext.CreateConnection();
        var booksQuery = await connection.QueryAsync<Book, Rating, Book>(
            $""" SELECT * FROM "Books" as b LEFT JOIN "Ratings" as r ON b."BookId"=r."BookId"  ORDER BY "{SelectOrderProperty()}" """,
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
        var books = booksQuery.Distinct().ToList();
        return books.Select(b => _mapper.Map<ListedBookResponseDTO>(b));
    }
}