using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Core.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IBeersService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IReadOnlyList<BeerRatingsResponse>> GetBeersAsync(QueryFilter queryFilter, CancellationToken cancellationToken);

        /// <summary>
        ///
        /// </summary>
        /// <param name="beerRating"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddRatingAsync(BeerRating beerRating, CancellationToken cancellationToken);
    }
}