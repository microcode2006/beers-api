using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vintri.Beers.Core.Models;
using OneOf;

namespace Vintri.Beers.Core.Interfaces
{
    /// <summary>
    /// Punk API client
    /// </summary>
    public interface IPunkClient
    {
        /// <summary>
        /// Get beers from Punk API
        /// </summary>
        /// <param name="queryOption">Either beer id or beer name with pagination</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of beers</returns>
        Task<IReadOnlyList<Beer>> GetBeersAsync(OneOf<int, QueryFilter> queryOption, CancellationToken cancellationToken);
    }
}