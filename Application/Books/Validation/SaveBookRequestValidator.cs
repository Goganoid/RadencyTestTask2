using Application.Books.DTO;
using Application.Books.DTO.Requests;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
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
            .MustAsync(async (id, token) => await HaveValidId(id!.Value))
            .When(b => b.Id != null)
            .WithMessage("The book with given id does not exist");
        RuleFor(b => b.Author).NotNull().Length(4, 50);
        RuleFor(b => b.Content).NotNull().Length(25, 1000);
        RuleFor(b => b.Cover).NotNull().Must(c =>
        {
            // correct format data:image/jpeg;base64,...base64......
            var split = c.Split(',');
            if (split.Length < 2) return false;
            var (format,base64) = (split[0], split[1]);
            if (string.IsNullOrEmpty(base64) || base64.Length % 4 != 0
                || base64.Contains(" ") || base64.Contains("\t")
                || base64.Contains("\r") || base64.Contains("\n")
                || !format.Contains("data:image"))
                return false;
            // check image is less than 2MB
            var fileSizeInByte = Math.Ceiling((double)base64.Length / 4) * 3;
            if (fileSizeInByte > 2 * Math.Pow(10, 6)) return false;
            var buffer = new Span<byte>(new byte[base64!.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }).WithMessage("Cover image is in incorrect format");
        RuleFor(b => b.Title).NotNull().Length(4, 50);
        RuleFor(b => b.Genre).NotNull().Must(g => Constants.Genres.Contains(g)).WithMessage("The genre is incorrect");
    }

    private async Task<bool> HaveValidId(int bookId)
    {
        var book = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
        return book != null;
    }
}