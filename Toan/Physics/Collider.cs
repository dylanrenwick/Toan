using Microsoft.Xna.Framework;

using Toan.ECS.Components;

namespace Toan.Physics;

public struct Collider : IComponent
{
    public required ColliderShape Shape { get; init; }

    public required Vector2 Origin { get; set; }

    public required CollisionMask Mask { get; set; }
    
    public required ulong Layer { get; set; }

    public bool IsCircle
    => Shape switch
    {
        ColliderShape.Circle or ColliderShape.Point => true,
        _ => false,
    };

    public bool IsRect
        => Shape == ColliderShape.Rect;
}
