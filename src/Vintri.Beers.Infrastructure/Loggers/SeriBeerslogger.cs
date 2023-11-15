using System;
using Vintri.Beers.Core.Interfaces;

namespace Vintri.Beers.Infrastructure.Loggers;

internal class SeriBeerslogger : IBeersLogger
{
    private readonly Serilog.ILogger _logger;

    public SeriBeerslogger(Serilog.ILogger logger) => _logger = logger;

    public void LogException(Exception exception, string message) => _logger.Error(exception, message);

    public void LogInformation(string message) => _logger.Information(message);
}