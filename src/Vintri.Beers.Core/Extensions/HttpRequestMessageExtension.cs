using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Vintri.Beers.Core.Models;

namespace Vintri.Beers.Core.Extensions;

public static class HttpRequestMessageExtension
{
    public static HttpResponseMessage CreateErrorResult(this HttpRequestMessage request, HttpStatusCode statusCode, string message) =>
        new(statusCode)
        {
            Content = new StringContent(JsonConvert.SerializeObject(
                new ErrorResult{
                    Message = message,
                    StatusCode = (int)statusCode
                }), Encoding.UTF8, "application/json")
        };
}