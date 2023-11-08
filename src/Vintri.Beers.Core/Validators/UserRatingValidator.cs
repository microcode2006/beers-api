using FluentValidation;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Core.Validators;

public class UserRatingValidator : AbstractValidator<UserRating>
{
    public UserRatingValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Please specify a valid email address");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Please specify a valid rating from 1 to 5");

        RuleFor(x => x.Comments)
            .MaximumLength(50)
            .WithMessage("Please specify a comment with a maximum of 200");
    }
}