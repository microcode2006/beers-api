using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Core.Interfaces
{
    /// <summary>
    /// The repository interface that is used for CRUD operations for beer ratings
    /// </summary>
    public interface IBeerRatingRepository
    {
        /// <summary>
        /// Add user rating for a beer
        /// </summary>
        /// <param name="beerRating">Beer rating added by user</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync(BeerRating beerRating, CancellationToken cancellationToken);

        /// <summary>
        /// Get user ratings for beers with provided beer ids
        /// </summary>
        /// <param name="beerIds">A list of beer ids</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of beer ratings</returns>
        Task<IReadOnlyList<BeerRatings>> GetAsync(List<int> beerIds, CancellationToken cancellationToken);
    }
}