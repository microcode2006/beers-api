using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        var statusCode = GetStatusCode(exception);
        actionExecutedContext.Response = CreateHttpResponseMessage(exception.Message, statusCode);
    }

    private HttpStatusCode GetStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException or InvalidUsernameException => HttpStatusCode.BadRequest,
            TaskCanceledException or OperationCanceledException => HttpStatusCode.RequestTimeout,
            _ => HttpStatusCode.InternalServerError,
        };

    private HttpResponseMessage CreateHttpResponseMessage(string message, HttpStatusCode statusCode) =>
        new(statusCode)
        {
            Content = new StringContent(FormatAsJson(new ErrorResult
            {
                Message = message,
                StatusCode = (int)statusCode
            })),
        };

    private string FormatAsJson(ErrorResult errorResult) =>
        JsonConvert.SerializeObject(errorResult, Formatting.Indented, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
}