using System;

using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;

namespace Toan.Physics;

public static class CollisionHelper
{
    /// <summary>
    /// Utility method for calculating smallest AABB containing collider
    /// </summary>
    /// <param name="entity">The entity whos collider to use</param>
    /// <returns></returns>
    public static FloatRect GetColliderBoundingBox(Entity entity)
        => GetColliderBoundingBox(entity, ref entity.Get<Collider>());
    /// <summary>
    /// Utility method for calculating smallest AABB containing collider
    /// </summary>
    /// <param name="entity">The entity whos collider to use</param>
    /// <param name="collider">A reference to the <see cref="Collider"/> component</param>
    /// <returns></returns>
    public static FloatRect GetColliderBoundingBox(Entity entity, ref Collider collider)
        => GetColliderBoundingBox(entity, ref collider, ref entity.Get<Transform>());
    /// <summary>
    /// Utility method for calculating smallest AABB containing collider
    /// </summary>
    /// <param name="entity">The entity whos collider to use</param>
    /// <param name="collider">A reference to the <see cref="Collider"/> component</param>
    /// <param name="transform">A reference to the <see cref="Transform"/> component</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public static FloatRect GetColliderBoundingBox(Entity entity, ref Collider collider, ref Transform transform)
    => collider.Shape switch
    {
        ColliderShape.Circle => GetCircleBoundingBox(ref entity.Get<CircleCollider>()).Offset(transform.Position),
        ColliderShape.Rect   => throw new NotImplementedException(),
        _                    => new FloatRect(transform.Position, Vector2.Zero),
    };

    private static FloatRect GetCircleBoundingBox(ref CircleCollider circle)
        => new(new(-circle.Radius), new(circle.Radius * 2f));
}
