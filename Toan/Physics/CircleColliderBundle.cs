using System;

using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Bundles;
using Toan.ECS.Components;

namespace Toan.Physics;

public struct CircleColliderBundle : IBundle
{
    public required float Radius { get; init; }

    public Vector2 Origin { get; init; }

    public ulong Layer { get; init; }

    public CollisionMask Mask { get; init; }

    public void AddBundle(Entity entity)
    {
        entity.WithIfNew(new Collider()
        {
            Layer  = Layer,
            Mask   = Mask,
            Origin = Origin,
            Shape  = ColliderShape.Circle,
        })
        .WithIfNew(new CircleCollider()
        {
            Radius = Radius
        });
    }
}
