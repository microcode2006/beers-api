using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Web.Http;
using Vintri.Beers.Core;

namespace Vintri.Beers.Api.Controllers;

/// <summary>
/// A simple health check controller used for health check endpoint
/// </summary>
[ApiVersion("1")]
public class HealthCheckController : ApiController
{
    private readonly HealthCheckService _healthCheckService;

    /// <inheritdoc />
    public HealthCheckController(HealthCheckService healthCheckService) => _healthCheckService = healthCheckService;

    /// <summary>
    /// Health check
    /// </summary>
    /// <returns>Health check status and report</returns>
    [HttpGet]
    [Route($"api/v{{version:{Constants.ApiVersion}}}/health")]
    public async Task<IHttpActionResult> GetAsync()
    {
        var healthReport = await _healthCheckService.CheckHealthAsync().ConfigureAwait(false);

        return healthReport.Status == HealthStatus.Healthy
            ? Ok(healthReport)
            : NotFound();
    }


}