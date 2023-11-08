using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using FluentValidation;
using Newtonsoft.Json;
using Vintri.Beers.Core.Exceptions;
using Vintri.Beers.Core.Interfaces;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Api.Attributes;

/// <summary>
/// Exception filter to log exceptions and return proper error result
/// </summary>
public class ExceptionHandlingAttribute : ExceptionFilterAttribute
{
    private readonly IBeersLogger _logger;

    /// <inheritdoc />
    public ExceptionHandlingAttribute(IBeersLogger logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public override void OnException(HttpActionExecutedContext actionExecutedContext)
    {
        var exception = actionExecutedContext.Exception;

        _logger.LogException(exception,
            $"An exception occurred from request: {actionExecutedContext.Request.Method} {actionExecutedContext.Request.RequestUri}");

        var statusCode = exception switch
        {
            ValidationException or InvalidUsernameException => HttpStatusCode.BadRequest,
            TaskCanceledException or OperationCanceledException => HttpStatusCode.RequestTimeout,
            _ => HttpStatusCode.InternalServerError,
        };

        actionExecutedContext.Response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new ErrorResult
            {
                Message = exception.Message,
                StatusCode = (int)statusCode
            }), Encoding.UTF8, "application/json")
        };
    }

}