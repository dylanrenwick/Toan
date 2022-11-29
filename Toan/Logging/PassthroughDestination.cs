namespace Toan.Logging;

public abstract class PassthroughDestination<T1, T2> : ILogDestination<T1>
{
    public required ILogDestination<T2> Destination { get; init; }

    public virtual void Log(T1 message)
        => Destination.Log(Convert(message));

    public abstract T2 Convert(T1 message);
}

