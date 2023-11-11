using System.Web.Http.Filters;
using Vintri.Beers.Core.Extensions;
using Vintri.Beers.Core.Interfaces;

namespace Vintri.Beers.Api.Attributes;

/// <summary>
/// Exception filter to log exceptions and return proper error result
/// </summary>
public class ExceptionHandlingAttribute : ExceptionFilterAttribute
{
    private readonly IBeersLogger _logger;

    /// <inheritdoc />
    public ExceptionHandlingAttribute(IBeersLogger logger) => _logger = logger;

    /// <inheritdoc />
    public override void OnException(HttpActionExecutedContext actionExecutedContext)
    {
        var exception = actionExecutedContext.Exception;

        _logger.LogException(exception,
            $"An exception occurred from request: {actionExecutedContext.Request.Method} {actionExecutedContext.Request.RequestUri}");

        actionExecutedContext.Response = exception.ToHttpResponseMessage();
    }

}