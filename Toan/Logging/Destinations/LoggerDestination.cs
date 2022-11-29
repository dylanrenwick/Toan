namespace Toan.Logging.Destinations;

public class LoggerDestination : ILogDestination<LogMessage>
{
    public required Logger Logger { get; init; }

    public void Log(LogMessage message)
        => Logger.Log(message);
}

