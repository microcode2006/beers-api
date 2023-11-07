using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Vintri.Beers.Core.Interfaces;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Core.Validators;

public class BeerRatingValidator : AbstractValidator<BeerRating>
{
    public BeerRatingValidator(IPunkClient punkClient)
    {
        RuleFor(x => x.UserRating).SetValidator(new UserRatingValidator()!);

        RuleFor(x => x.BeerId)
            .NotEmpty()
            .GreaterThan(0)
            .MustAsync(
                async(beerId, cancellationToken) =>
                await BeerExistsAsync(beerId, punkClient, cancellationToken).ConfigureAwait(false)
             )
            .WithMessage("Please specify a valid beer id");
    }

    private async Task<bool> BeerExistsAsync(int beerId, IPunkClient punkClient, CancellationToken cancellationToken)
    {
        var beers = await punkClient.GetBeersAsync(beerId, cancellationToken).ConfigureAwait(false);
        return beers.Any();
    }

}