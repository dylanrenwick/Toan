using Toan.ECS.Resources;

namespace Toan.Logging.Destinations;

public class TextLogDestination : ILogDestination<string>
{
    public required TextLog TextLog { get; init; }

    public void Log(string message)
        => TextLog.Log(message);
}

