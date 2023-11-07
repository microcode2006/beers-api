using System.Collections.Generic;
using System.Threading;
using Microsoft.Web.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Vintri.Beers.Api.Attributes;
using Vintri.Beers.Core;
using Vintri.Beers.Core.Models;
using Vintri.Beers.Core.Interfaces;

namespace Vintri.Beers.Api.Controllers
{
    [ApiVersion("1")]
    [RoutePrefix($"api/v{{version:{Constants.ApiVersion}}}/beers")]
    public class BeersController : ApiController
    {
        private readonly IBeersService _beersService;

        public BeersController(IBeersService beersService)
        {
            _beersService = beersService;
        }

        [HttpGet]
        [ResponseType(typeof(IReadOnlyList<BeerRatingsResponse>))]
        [Route("")]
        public async Task<IHttpActionResult> GetBeersAsync([FromUri]QueryFilter queryFilter, CancellationToken cancellationToken)
        {
            var beers = await _beersService.GetBeersAsync(queryFilter, cancellationToken).ConfigureAwait(false);
            return Ok(beers);
        }

        /// <summary>
        /// Add rating to beer with beerId
        /// </summary>
        /// <param name="beerId">beer id</param>
        /// <param name="userRating"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateUsername]
        [ResponseType(typeof(BeerRating))]
        [Route("{beerId}/ratings")]
        public async Task<IHttpActionResult> AddRatingAsync(int beerId, [FromBody]UserRating userRating, CancellationToken cancellationToken)
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