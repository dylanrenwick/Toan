namespace Toan.Logging.Destinations;

public interface ILogDestination<T>
{
    public void Log(T message);
}

