using System.Collections.Generic;

namespace Toan.Logging.Color;

public abstract class ColorConverter
{
    private static readonly IReadOnlyDictionary<LogLevel, LogColor> _levelColors = new Dictionary<LogLevel, LogColor>()
    {
        [LogLevel.Debug] = LogColor.Cyan | LogColor.Bright,
        [LogLevel.Info]  = LogColor.White,
        [LogLevel.Warn]  = LogColor.Yellow,
        [LogLevel.Error] = LogColor.Red,
        [LogLevel.Fatal] = LogColor.Red | LogColor.Bright,
    };

    public abstract string Convert(LogColor color);
    public virtual string ConvertLevel(LogLevel level)
        => Convert(LevelToColor(level));
    public virtual string Reset()
        => Convert(LogColor.Reset);

    protected static LogColor LevelToColor(LogLevel level)
        => _levelColors[level];
}

