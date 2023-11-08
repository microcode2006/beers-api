using System.Collections.Generic;
using System.Net;
using System.Threading;
using Microsoft.Web.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using Vintri.Beers.Api.Attributes;
using Vintri.Beers.Core;
using Vintri.Beers.Core.Models;
using Vintri.Beers.Core.Interfaces;

namespace Vintri.Beers.Api.Controllers
{
    /// <inheritdoc />
    [ApiVersion("1")]
    [RoutePrefix($"api/v{{version:{Constants.ApiVersion}}}/beers")]
    public class BeersController : ApiController
    {
        private readonly IBeersService _beersService;

        /// <inheritdoc />
        public BeersController(IBeersService beersService)
        {
            _beersService = beersService;
        }

        /// <summary>
        /// Get all beers that match the beerName and paging query string
        /// </summary>
        /// <param name="queryFilter">Query parameters to specify beerName, page and perPage</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A list of paged beers that match the beerName query string</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, "Returns a list of paged beers that match the beerName query string ", typeof(IReadOnlyList<BeerRatingsResponse>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Invalid beer name or paging parameters")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal errors occurred")]
        [SwaggerResponse(HttpStatusCode.RequestTimeout, "Request timed out or cancelled")]
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
        [SwaggerResponse(HttpStatusCode.Created, "The user rating created for beer", typeof(BeerRating))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Invalid beer id or user rating")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Internal errors occurred")]
        [SwaggerResponse(HttpStatusCode.RequestTimeout, "Request timed out or cancelled")]
        [Route("{beerId}/ratings")]
        public async Task<IHttpActionResult> AddRatingAsync(int beerId, [FromBody]UserRating userRating, CancellationToken cancellationToken = default)
        {
            var beerRating = new BeerRating
            {
                BeerId = beerId,
                UserRating = userRating
            };
            await _beersService.AddRatingAsync(beerRating, cancellationToken).ConfigureAwait(false);

            return Created(string.Empty, beerRating);
        }
    }
}