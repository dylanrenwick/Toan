using System.Collections.Generic;

namespace Toan.Logging.Color;

public class AnsiColorConverter : ColorConverter
{
    private const string ANSI_COLOR_FORMAT = "\u001b[{0}m";

    private static readonly IReadOnlyDictionary<LogColor, string> _colorCodes = new Dictionary<LogColor, string>()
    {
        [LogColor.Reset]   = "0",
        [LogColor.Bright]  = "1",
        [LogColor.Black]   = "30",
        [LogColor.Red]     = "31",
        [LogColor.Green]   = "32",
        [LogColor.Yellow]  = "33",
        [LogColor.Blue]    = "34",
        [LogColor.Magenta] = "35",
        [LogColor.Cyan]    = "36",
        [LogColor.White]   = "37",
    };

    public override string Convert(LogColor color)
        => AnsiCodeFromColor(color);

    private static string AnsiCodeFromColor(LogColor color)
    {
        bool isBright = (color & LogColor.Bright) == LogColor.Bright;
        string colorCode = _colorCodes[isBright ? color ^ LogColor.Bright : color];

        if (isBright)
            colorCode += $";{_colorCodes[LogColor.Bright]}";

        return string.Format(ANSI_COLOR_FORMAT, colorCode);
    }
}

