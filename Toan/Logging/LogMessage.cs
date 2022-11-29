using System;
using System.Text;

using Toan.Logging.Color;

namespace Toan.Logging;

public class LogMessage
{
    public required string Label { get; init; }
    public required LogLevel Level { get; init; }
    public required string Message { get; init; }
    public required DateTime Timestamp { get; init; }

    public string ToString(ColorConverter color)
        => $"{Timestamp:s} | {color.ConvertLevel(Level)}{Level.ToString().ToUpper()} {Label} |> {Message}";
}

