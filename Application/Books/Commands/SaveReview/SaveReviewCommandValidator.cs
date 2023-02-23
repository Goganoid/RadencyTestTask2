using FluentValidation;

namespace Application.Books.Commands;

public class SaveReviewCommandValidator : AbstractValidator<SaveReviewRequestDTO>
{
    public SaveReviewCommandValidator()
    {
        RuleFor(r => r.Message).NotNull().Length(10, 250);
        RuleFor(r => r.Reviewer).NotNull().Length(4, 50);
    }
}