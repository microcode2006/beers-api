using System;
using System.Net;
using System.Net.Http;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Vintri.Beers.Core.Exceptions;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Core.Extensions;

public static class ExceptionExtension
{
    public static HttpResponseMessage ToHttpResponseMessage(this Exception exception) =>
        CreateHttpResponseMessage(exception.Message, GetStatusCode(exception));

    private static HttpStatusCode GetStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException or InvalidUsernameException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };

    private static HttpResponseMessage CreateHttpResponseMessage(string message, HttpStatusCode statusCode) =>
        new(statusCode)
        {
            Content = new StringContent(FormatAsJson(new ErrorResult
            {
                Message = message,
                StatusCode = (int)statusCode
            })),
        };

    private static string FormatAsJson(ErrorResult errorResult) =>
        JsonConvert.SerializeObject(errorResult, Formatting.Indented, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
}