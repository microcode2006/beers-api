using System.Web.Http.Filters;
using Vintri.Beers.Core.Interfaces;

namespace Vintri.Beers.Api.Attributes;

/// <summary>
/// Log request and response from every action
/// </summary>
public class LogRequestAttribute : ActionFilterAttribute
{
    private readonly IBeersLogger _logger;

    /// <inheritdoc />
    public LogRequestAttribute(IBeersLogger logger) => _logger = logger;

    /// <inheritdoc />
    public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
    {
        if (actionExecutedContext.Response != null)
        {
            _logger.LogInformation(
                $"Request: {actionExecutedContext.Request.Method} {actionExecutedContext.Request.RequestUri} | Response status code: {actionExecutedContext.Response.StatusCode}");
        }
    }

}