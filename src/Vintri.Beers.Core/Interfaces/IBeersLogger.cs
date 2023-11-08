using System;

namespace Vintri.Beers.Core.Interfaces;

/// <summary>
/// The interface that is used for logging
/// </summary>
public interface IBeersLogger
{
    /// <summary>
    /// Log information
    /// </summary>
    /// <param name="message">The message to be logged</param>
    void LogInformation(string message);

    /// <summary>
    /// Log exception
    /// </summary>
    /// <param name="exception">The exception to be logged</param>
    /// <param name="message">The message to be logged</param>
    void LogException(Exception exception, string message);
}