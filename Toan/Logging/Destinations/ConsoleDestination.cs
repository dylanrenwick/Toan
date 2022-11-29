using System;

namespace Toan.Logging.Destinations;

public class ConsoleDestination : ILogDestination<string>
{
    public void Log(string message)
        => Console.WriteLine(message);
}

