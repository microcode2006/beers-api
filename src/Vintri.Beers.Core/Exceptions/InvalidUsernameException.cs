using System;

namespace Vintri.Beers.Core.Exceptions;

public class InvalidUsernameException : Exception
{
    public InvalidUsernameException(string message) : base(message){ }
}