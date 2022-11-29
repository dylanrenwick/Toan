using Toan.ECS.Resources;

namespace Toan.Logging;

public class TextLogDestination : ILogDestination<string>
{
    public required TextLog TextLog { get; init; }

    public void Log(string message)
        => TextLog.Log(message);
}

