namespace Toan.Logging.Destinations;

public class LevelLimitedDestination : PassthroughDestination<LogMessage, LogMessage>
{
    public required LogLevel LevelLimit { get; init; }

    public override void Log(LogMessage message)
    {
        if (message.Level >= LevelLimit)
            base.Log(message);
    }

    public override LogMessage Convert(LogMessage message)
        => message;
}

