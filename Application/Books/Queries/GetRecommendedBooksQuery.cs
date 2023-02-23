using Application.Books.Commands.Core.Exceptions;
using AutoMapper;
using Dapper;
using Domain;
using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Books.Queries;

public record GetRecommendedBooksQuery(string? Genre) : IRequest<IEnumerable<ListedBookResponseDTO>>;

public class GetRecommendedBooks : IRequestHandler<GetRecommendedBooksQuery,IEnumerable<ListedBookResponseDTO>>
{
    private readonly DapperContext _dapperContext;
    private readonly IMapper _mapper;

    public GetRecommendedBooks(DapperContext dapperContext, IMapper mapper)
    {
        _dapperContext = dapperContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ListedBookResponseDTO>> Handle(GetRecommendedBooksQuery request, CancellationToken cancellationToken)
    {
        if (request.Genre != null && Constants.Genres.Find(g => string.Equals(g, request.Genre)) == null)
            throw new NotFoundException("Genre not found");
        var bookDictionary = new Dictionary<int, Book>();
        using var connection = _dapperContext.CreateConnection();
        var builder = new SqlBuilder();

        if (!string.IsNullOrEmpty(request.Genre)) builder.Where($""" b."Genre"= '{request.Genre}' """);
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
}