using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Core.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IUserRatingRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beerRating"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync(BeerRating beerRating, CancellationToken cancellationToken);

        /// <summary>
        ///
        /// </summary>
        /// <param name="beerIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IReadOnlyList<BeerRatings>> GetAsync(List<int> beerIds, CancellationToken cancellationToken);
    }
}