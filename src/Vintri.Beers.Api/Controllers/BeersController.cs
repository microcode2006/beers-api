using System.Threading;
using Microsoft.Web.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Vintri.Beers.Api.Attributes;
using Vintri.Beers.Core;
using Vintri.Beers.Core.Models;
using Vintri.Beers.Core.Interfaces;

namespace Vintri.Beers.Api.Controllers;

/// <inheritdoc />
[ApiVersion("1")]
[RoutePrefix($"api/v{{version:{Constants.ApiVersion}}}/beers")]
public class BeersController : ApiController
{
    private readonly IBeersService _beersService;

    /// <inheritdoc />
    public BeersController(IBeersService beersService) => _beersService = beersService;

    /// <summary>
    /// Get all beers that match the beerName and paging query string
    /// </summary>
    /// <param name="queryFilter">Query parameters to specify beerName, page and perPage</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of paged beers that match the beerName query string</returns>
    [HttpGet]
    [Route("")]
    public async Task<IHttpActionResult> GetBeersAsync([FromUri]QueryFilter queryFilter, CancellationToken cancellationToken = default)
    {
        var beers = await _beersService.GetBeersAsync(queryFilter, cancellationToken).ConfigureAwait(false);
        return Ok(beers);
    }

    /// <summary>
    /// Add a rating to a beer
    /// </summary>
    /// <param name="beerId">Beer id rated by user</param>
    /// <param name="userRating">User rating details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The posted user rating with beer id</returns>
    [HttpPost]
    [ValidateUsername]
    [Route("{beerId}/ratings")]
    public async Task<IHttpActionResult> AddRatingAsync(int beerId, [FromBody]UserRating userRating, CancellationToken cancellationToken = default)
    {
        var beerRating = new BeerRating(beerId, userRating);
        await _beersService.AddRatingAsync(beerRating, cancellationToken).ConfigureAwait(false);

        return Created(string.Empty, beerRating);
    }
}