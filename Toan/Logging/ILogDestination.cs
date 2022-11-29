namespace Toan.Logging;

public interface ILogDestination<T>
{
    public void Log(T message);
}

