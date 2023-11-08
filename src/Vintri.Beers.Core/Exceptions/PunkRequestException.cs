using System;
using System.Net.Http;

namespace Vintri.Beers.Core.Exceptions;

public class PunkRequestException : Exception
{
    public HttpResponseMessage Response { get; }

    public PunkRequestException(HttpResponseMessage response) : base($"Punk request failed: {response.ReasonPhrase}")
    {
        Response = response;
    }
}