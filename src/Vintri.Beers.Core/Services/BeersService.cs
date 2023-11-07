using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Vintri.Beers.Core.Interfaces;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Core.Services
{
    public class BeersService : IBeersService
    {
        private readonly IUserRatingRepository _userRatingRepository;
        private readonly IPunkClient _punkClient;
        private readonly IValidator<BeerRating> _beerRatingValidator;
        private readonly IValidator<QueryFilter> _queryFilterValidator;

        public BeersService(IUserRatingRepository userRatingRepository, IPunkClient punkClient,
            IValidator<BeerRating> beerRatingValidator, IValidator<QueryFilter> queryFilterValidator)
        {
            _userRatingRepository = userRatingRepository;
            _punkClient = punkClient;
            _beerRatingValidator = beerRatingValidator;
            _queryFilterValidator = queryFilterValidator;
        }

        public async Task<IReadOnlyList<BeerRatingsResponse>> GetBeersAsync(QueryFilter queryFilter, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _queryFilterValidator.ValidateAndThrowAsync(queryFilter, cancellationToken).ConfigureAwait(false);

            var beers = await _punkClient.GetBeersAsync(queryFilter, cancellationToken).ConfigureAwait(false);
            var beerIds = beers.Select(x => x.Id).ToList();
            var beerRatings = await _userRatingRepository.GetAsync(beerIds, cancellationToken).ConfigureAwait(false);

            var beerRatingsResponse = beers.Select(x => new BeerRatingsResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UserRatings = beerRatings.FirstOrDefault(y => y.Id == x.Id)?.UserRatings ?? new List<UserRating>()
            }).ToList();

            return beerRatingsResponse;
        }

        public async Task AddRatingAsync(BeerRating beerRating, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _beerRatingValidator.ValidateAndThrowAsync(beerRating, cancellationToken).ConfigureAwait(false);
            await _userRatingRepository.AddAsync(beerRating, cancellationToken).ConfigureAwait(false);
        }
    }
}