using System;
using System.Net.Http;

namespace Vintri.Beers.Core.Exceptions;

public class PunkRequestException : Exception
{
    public PunkRequestException(HttpResponseMessage response) : base(
        $"Punk request failed with statusCode: {response.StatusCode} reasonPhrase: {response.ReasonPhrase}") {}
}