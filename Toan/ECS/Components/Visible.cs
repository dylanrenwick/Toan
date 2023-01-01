using Toan.Util;

namespace Toan.ECS.Components;

public struct Visible : IComponent, ICloneable<Visible>
{
    public Visible Clone() => new();
}

