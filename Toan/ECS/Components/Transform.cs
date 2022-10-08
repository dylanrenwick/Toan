using Microsoft.Xna.Framework;

namespace Toan.ECS.Components;

public class Transform : GameComponent, ICloneable<Transform>
{
    public Vector2 LocalPosition { get; set; } = Vector2.Zero;
    public Vector2 GlobalPosition
    {
        get => LocalPosition + ParentPosition;
        set => LocalPosition = value - ParentPosition;
    }

    public Vector2 LocalScale { get; set; } = Vector2.One;
    public Vector2 GlobalScale
    {
        get => LocalScale * ParentScale;
        set => LocalScale = value / ParentScale;
    }

    public float LocalRotation { get; set; } = 0f;
    public float GlobalRotation
    {
        get => LocalRotation + ParentRotation;
        set => LocalRotation = value - ParentRotation;
    }

    public Transform? Parent { get; set; }

    private Vector2 ParentPosition => Parent?.GlobalPosition ?? Vector2.Zero;
    private Vector2 ParentScale => Parent?.GlobalPosition ?? Vector2.One;
    private float ParentRotation => Parent?.GlobalRotation ?? 0f;

    public Transform Clone() => new()
    {
        LocalPosition = LocalPosition,
        LocalRotation = LocalRotation,
        LocalScale = LocalScale,
        Parent = Parent,
    };
}
