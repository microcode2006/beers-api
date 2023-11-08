using FluentValidation;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Core.Validators;

public class QueryFilterValidator : AbstractValidator<QueryFilter>
{
    public QueryFilterValidator()
    {
        RuleFor(x => x.BeerName)
            .NotEmpty()
            .WithMessage("Please specify beer name.");

        RuleFor(x => x.PerPage)
            .InclusiveBetween(1, Constants.MaxPerPage)
            .WithMessage("Please specify a valid per page value from 1 to 25");

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Please specify a valid page value greater than 0");
    }
}