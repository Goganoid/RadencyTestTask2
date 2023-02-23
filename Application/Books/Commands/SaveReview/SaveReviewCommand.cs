using Application.Books.Commands.Core.Models;
using AutoMapper;
using Domain;
using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Books.Commands;

public record SaveReviewCommand(int BookId, SaveReviewRequestDTO ReviewDTO) : IRequest<IdResponseDTO>;

public class SaveReviewCommandHandler: BookCommand,IRequestHandler<SaveReviewCommand,IdResponseDTO>
{
    public SaveReviewCommandHandler(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }

    public async Task<IdResponseDTO> Handle(SaveReviewCommand request, CancellationToken cancellationToken)
    {
        var book = await GetBook(request.BookId);
        var review = Mapper.Map<Review>(request.ReviewDTO);
        book.Reviews.Add(review);
        await DataContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<IdResponseDTO>(review);
    }
}