using System.Net;
using System.Web.Http.Filters;
using FluentValidation;
using Vintri.Beers.Core.Extensions;

namespace Vintri.Beers.Api.Attributes;

/// <summary>
/// Exception filter to handle any uncaught exceptions
/// </summary>
public class ErrorHandlingAttribute : ExceptionFilterAttribute
{
    /// <inheritdoc />
    public override void OnException(HttpActionExecutedContext actionExecutedContext)
    {
        var errorMessage = actionExecutedContext.Exception.Message;
        var statusCode = actionExecutedContext.Exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };

       actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResult(statusCode, errorMessage);
    }

}