using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Vintri.Beers.Core.Interfaces;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Core.Services
{
    internal class BeersService : IBeersService
    {
        private readonly IBeerRatingRepository _beerRatingRepository;
        private readonly IPunkClient _punkClient;
        private readonly IValidator<BeerRating> _beerRatingValidator;
        private readonly IValidator<QueryFilter> _queryFilterValidator;

        public BeersService(IBeerRatingRepository beerRatingRepository, IPunkClient punkClient,
            IValidator<BeerRating> beerRatingValidator, IValidator<QueryFilter> queryFilterValidator)
        {
            _punkClient = punkClient;
            _beerRatingRepository = beerRatingRepository;
            _beerRatingValidator = beerRatingValidator;
            _queryFilterValidator = queryFilterValidator;
        }

        // We may return better paging results following json:api specification: https://jsonapi.org/
        public async Task<IReadOnlyList<BeerRatingsResponse>> GetBeersAsync(QueryFilter queryFilter, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _queryFilterValidator.ValidateAndThrowAsync(queryFilter, cancellationToken).ConfigureAwait(false);

            var beers = await _punkClient.GetBeersAsync(queryFilter, cancellationToken).ConfigureAwait(false);
            var beerIds = beers.Select(x => x.Id).ToList();
            var beerRatings = await _beerRatingRepository.GetAsync(beerIds, cancellationToken).ConfigureAwait(false);

            var beerRatingsResponse = beers.Select(beer => new BeerRatingsResponse
            (
                Id: beer.Id, Name: beer.Name,
                Description: beer.Description,
                UserRatings: beerRatings.FirstOrDefault(beerRating => beerRating.Id == beer.Id)?.UserRatings ?? new List<UserRating>())
            ).ToList();

            return beerRatingsResponse;
        }

        public async Task AddRatingAsync(BeerRating beerRating, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _beerRatingValidator.ValidateAndThrowAsync(beerRating, cancellationToken).ConfigureAwait(false);
            await _beerRatingRepository.AddAsync(beerRating, cancellationToken).ConfigureAwait(false);
        }
    }
}