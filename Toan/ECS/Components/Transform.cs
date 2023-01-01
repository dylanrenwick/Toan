using Microsoft.Xna.Framework;
using Toan.Util;

namespace Toan.ECS.Components;

public struct Transform : IComponent, ICloneable<Transform>
{
    public Vector2 Position { get; set; } = Vector2.Zero;

    public Vector2 Scale { get; set; } = Vector2.One;

    public float Rotation { get; set; }

    public Transform() { }

    public Transform Clone() => new()
    {
        Position = Position,
        Rotation = Rotation,
        Scale = Scale,
    };
}
