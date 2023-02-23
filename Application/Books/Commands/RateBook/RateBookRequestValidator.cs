using FluentValidation;

namespace Application.Books.Commands;

public class RateBookRequestValidator : AbstractValidator<RateBookRequestDTO>
{
    public RateBookRequestValidator()
    {
        RuleFor(r => r.Score).NotNull().InclusiveBetween(1, 5);
    }
}