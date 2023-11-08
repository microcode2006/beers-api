using System.Collections.Generic;
using System.Net;
using System.Threading;
using Microsoft.Web.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using Vintri.Beers.Api.Attributes;
using Vintri.Beers.Core;
using Vintri.Beers.Core.Models;
using Vintri.Beers.Core.Interfaces;

namespace Vintri.Beers.Api.Controllers
{
    /// <summary>
    /// A simple health check controller used for health check endpoint
    /// </summary>
    public class HealthCheckController : ApiController
    {
        private readonly HealthCheckService _healthCheckService;

        /// <inheritdoc />
        public HealthCheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }


        /// <summary>
        /// Health check
        /// </summary>
        /// <returns>Health check status and report</returns>
        [HttpGet]
        [Route("api/health")]
        public async Task<IHttpActionResult> GetAsync()
        {
            var healthReport = await _healthCheckService.CheckHealthAsync().ConfigureAwait(false);
            var reportToJson = JsonConvert.SerializeObject(healthReport);

            return healthReport.Status == HealthStatus.Healthy ? Ok(reportToJson) : NotFound();
        }


    }
}