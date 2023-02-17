using Application.Books.DTO;
using Application.Books.DTO.Requests;
using FluentValidation;

namespace Application.Books.Validation;

public class RateBookRequestValidator : AbstractValidator<RateBookRequestDTO>
{
    public RateBookRequestValidator()
    {
        RuleFor(r => r.Score).NotNull().InclusiveBetween(1, 5);
    }
}