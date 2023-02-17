using Application.Books.DTO;
using Application.Books.DTO.Requests;
using FluentValidation;

namespace Application.Books.Validation;

public class SaveReviewRequestValidator : AbstractValidator<SaveReviewRequestDTO>
{
    public SaveReviewRequestValidator()
    {
        RuleFor(r => r.Message).NotNull().Length(10, 250);
        RuleFor(r => r.Reviewer).NotNull().Length(4, 50);
    }
}