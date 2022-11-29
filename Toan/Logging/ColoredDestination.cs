using Toan.Logging.Color;

namespace Toan.Logging;

public class ColoredDestination : PassthroughDestination<LogMessage, string>
{
    public required ColorConverter Color { get; init; }

    public override string Convert(LogMessage message)
        => message.ToString(Color);
}

