using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vintri.Beers.Core.Models;
using OneOf;

namespace Vintri.Beers.Core.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IPunkClient
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<Beer>> GetBeersAsync(OneOf<int, QueryFilter> queryOption,CancellationToken cancellationToken);
    }
}