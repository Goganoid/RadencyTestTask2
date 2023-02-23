using AutoMapper;
using Domain;
using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Books.Commands;

public record RateBookCommand(int BookId, RateBookRequestDTO RateDTO) : IRequest;

public class RateBookCommandHandler : BookCommand, IRequestHandler<RateBookCommand>
{
    public RateBookCommandHandler(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public async Task Handle(RateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await GetBook(request.BookId);
        var rating = Mapper.Map<Rating>(request.RateDTO);
        book.Ratings.Add(rating);
        await DataContext.SaveChangesAsync(cancellationToken);
    }
}