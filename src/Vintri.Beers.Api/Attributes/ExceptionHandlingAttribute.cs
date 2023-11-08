using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using FluentValidation;
using Newtonsoft.Json;
using Vintri.Beers.Core.Exceptions;
using Vintri.Beers.Core.Extensions;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Api.Attributes;

/// <summary>
/// Exception filter to handle any uncaught exceptions
/// </summary>
public class ExceptionHandlingAttribute : ExceptionFilterAttribute
{
    /// <inheritdoc />
    public override void OnException(HttpActionExecutedContext actionExecutedContext)
    {
        var message = actionExecutedContext.Exception.Message;
        var statusCode = actionExecutedContext.Exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };

        actionExecutedContext.Response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new ErrorResult
            {
                Message = message,
                StatusCode = (int)statusCode
            }), Encoding.UTF8, "application/json")
        };
    }

}