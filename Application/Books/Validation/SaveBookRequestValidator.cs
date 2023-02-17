using Application.Books.DTO;
using Application.Books.DTO.Requests;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Books.Validation;

public class SaveBookRequestValidator : AbstractValidator<SaveBookRequestDTO>
{
    private DataContext _dataContext;
    public SaveBookRequestValidator(DataContext dataContext)
    {
        _dataContext = dataContext;
        RuleFor(b => b.Id)
            .MustAsync(async (id,token) => await HaveValidId(id!.Value))
            .When(b => b.Id != null)
            .WithMessage("The book with given id does not exist");
        RuleFor(b => b.Author).NotNull().Length(4,50);
        RuleFor(b => b.Content).NotNull().Length(4,50);
        RuleFor(b => b.Cover).NotNull().Length(4,50);
        RuleFor(b => b.Title).NotNull().Length(4,50);
        RuleFor(b => b.Genre).NotNull().Must(g=>Constants.Genres.Contains(g)).WithMessage("The genre is incorrect");
    }

    private async Task<bool> HaveValidId(int bookId)
    {
        var book = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
        return book != null;
    }
}