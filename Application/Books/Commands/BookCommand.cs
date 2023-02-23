using Application.Books.Commands.Core.Exceptions;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Books.Commands;

public class BookCommand
{
    protected readonly DataContext DataContext;
    protected readonly IMapper Mapper;

    protected BookCommand(DataContext dataContext, IMapper mapper)
    {
        DataContext = dataContext;
        Mapper = mapper;
    }
    protected async Task<Book> GetBook(int bookId)
    {
        var book = await DataContext.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
        if (book == null) throw new NotFoundException("Book with the given ID was not found");
        return book;
    }
}