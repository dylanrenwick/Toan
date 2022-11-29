using System;
using System.Collections.Generic;

using Toan.ECS.Resources;
using Toan.Logging.Destinations;

namespace Toan.Logging;

public class Logger : Resource
{
    public required string Label { get; init; }

    public required ICollection<ILogDestination<LogMessage>> Destinations { get; init; }

    public void Debug(string message)
        => Log(LogLevel.Debug, message);
    public void Info(string message)
        => Log(LogLevel.Info, message);
    public void Warn(string message)
        => Log(LogLevel.Warn, message);
    public void Error(string message)
        => Log(LogLevel.Error, message);
    public void Fatal(string message)
        => Log(LogLevel.Fatal, message);

    public void Log(LogLevel logLevel, string message)
        => Log(BuildMessage(logLevel, message));
    public void Log(LogMessage message)
    {
        foreach (var dest in Destinations)
        {
            dest.Log(message);
        }
    }

    public virtual LogMessage BuildMessage(LogLevel logLevel, string message)
    => new()
    {
        Label     = Label,
        Level     = logLevel,
        Message   = message,
        Timestamp = DateTime.Now
    };

    public Logger GetChildLogger(string childLabel)
    => new()
    {
        Destinations = new List<ILogDestination<LogMessage>>()
        {
            new LoggerDestination { Logger = this },
        },
        Label = childLabel
    };
}

