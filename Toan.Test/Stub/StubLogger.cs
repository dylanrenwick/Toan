using Toan.Logging;
using Toan.Logging.Color;
using Toan.Logging.Destinations;

namespace Toan.Test.Stub;

public class StubLogger : Logger
{
    public static StubLogger Instance { get; private set; } = new()
    {
        Label = "STUB",
        Destinations = new List<ILogDestination<LogMessage>>()
        {
            new ColoredDestination()
            {
                Destination = new ConsoleDestination(),
                Color = new AnsiColorConverter(),
            },
        }
    };
}
