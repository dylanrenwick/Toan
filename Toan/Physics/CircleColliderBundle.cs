using System;

using Microsoft.Xna.Framework;

using Toan.ECS.Bundles;
using Toan.ECS.Components;

namespace Toan.Physics;
public struct CircleColliderBundle : IBundle
{
    public required float Radius { get; init; }

    public Vector2 Origin { get; init; }

    public ulong Layer { get; init; }

    public CollisionMask Mask { get; init; }

    public void AddBundle(Guid entityId, ComponentRepository componentRepo)
    {
        IBundle.AddIfNew<Collider>(entityId, componentRepo, new()
        {
            Layer  = Layer,
            Mask   = Mask,
            Origin = Origin,
            Shape  = ColliderShape.Circle,
        });
        IBundle.AddIfNew<CircleCollider>(entityId, componentRepo, new() { Radius = Radius });
    }
}
