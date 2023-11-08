using System;
using System.Net.Http;

namespace Vintri.Beers.Core.Exceptions;

public class InvalidUsernameException : Exception
{
    public InvalidUsernameException(string message) : base(message){ }
}