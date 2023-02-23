using Application.Books.Commands.Core.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Books.Commands;

public record SaveBookCommand(SaveBookRequestDTO BookDTO) : IRequest<IdResponseDTO>;

public class SaveBookCommandHandler : BookCommand,IRequestHandler<SaveBookCommand,IdResponseDTO>
{
    public SaveBookCommandHandler(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
    }
    public async Task<IdResponseDTO> Handle(SaveBookCommand request, CancellationToken cancellationToken)
    {
        var book = Mapper.Map<Book>(request.BookDTO);
        var existingBook = await DataContext.Books.FirstOrDefaultAsync(b => b.BookId == request.BookDTO.BookId, cancellationToken: cancellationToken);
        if (existingBook != null)
        {
            existingBook.Author = book.Author;
            existingBook.Content = book.Content;
            existingBook.Cover = book.Cover;
            existingBook.Genre = book.Genre;
            existingBook.Title = book.Title;
            DataContext.Books.Update(existingBook);
        }
        else
        {
            await DataContext.Books.AddAsync(book, cancellationToken);
        }

        await DataContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<IdResponseDTO>(book);
    }

    
}