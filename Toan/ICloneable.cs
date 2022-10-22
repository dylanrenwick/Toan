namespace Toan;

/// <summary>
/// Exposes a Clone() method, which returns a value of type T
/// </summary>
/// <typeparam name="T">The type to clone into</typeparam>
public interface ICloneable<T>
    where T : ICloneable<T>
{
    public T Clone();
}
