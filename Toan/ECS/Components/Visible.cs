namespace Toan.ECS.Components;

public class Visible : GameComponent, ICloneable<Visible>
{
    public Visible Clone() => new();
}

