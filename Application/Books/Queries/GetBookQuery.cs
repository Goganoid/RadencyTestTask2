using Application.Books.Commands.Core.Exceptions;
using AutoMapper;
using Dapper;
using Domain;
using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Books.Queries;

public record GetBookQuery(int BookId) : IRequest<BookDetailsResponseDTO>;

public class GetBook : IRequestHandler<GetBookQuery, BookDetailsResponseDTO>
{
    private readonly DapperContext _dapperContext;
    private readonly IMapper _mapper;

    public GetBook(DapperContext dapperContext, IMapper mapper)
    {
        _dapperContext = dapperContext;
        _mapper = mapper;
    }

    public async Task<BookDetailsResponseDTO> Handle(GetBookQuery request, CancellationToken cancellationToken)
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
        }, new {request.BookId}, splitOn: "RatingId");
        var book = bookQuery.FirstOrDefault();
        if (book == null) throw new NotFoundException("Book with the given ID was not found");

        var reviewsQuery = await connection.QueryAsync<Review>(
            """
    SELECT * FROM "Reviews" as r
    WHERE  r."BookId"=@bookId
""", new {request.BookId});
        book.Reviews = reviewsQuery.ToList();


        return _mapper.Map<BookDetailsResponseDTO>(book);
    }
};