using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Core.Interfaces
{
    /// <summary>
    /// The domain service interface that includes all business logic for beers
    /// </summary>
    public interface IBeersService
    {
        /// <summary>
        /// Ger beers and related ratings
        /// </summary>
        /// <param name="queryFilter">Query parameters for beer name, pagination</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IReadOnlyList<BeerRatingsResponse>> GetBeersAsync(QueryFilter queryFilter, CancellationToken cancellationToken);

        /// <summary>
        /// Add user rating for a beer
        /// </summary>
        /// <param name="beerRating">Beer rating added by user</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddRatingAsync(BeerRating beerRating, CancellationToken cancellationToken);
    }
}