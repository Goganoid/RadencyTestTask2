using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Books.Commands;


public record DeleteBookCommand(int BookId) : IRequest;

public class DeleteBookCommandHandler : BookCommand,IRequestHandler<DeleteBookCommand>
{
    public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await GetBook(request.BookId);
        DataContext.Books.Remove(book);
        await DataContext.SaveChangesAsync(cancellationToken);
    }

    public DeleteBookCommandHandler(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
    { }
}