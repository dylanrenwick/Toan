using System;

using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;

namespace Toan.Physics;

public static class CollisionHelper
{
    public static Vector2 GetColliderOrigin(Entity entity)
        => GetColliderOrigin(ref entity.Get<Transform>(), ref entity.Get<Collider>());

    public static Vector2 GetColliderOrigin(ref Transform transform, ref Collider collider)
        => transform.Position + collider.Origin;

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
        ColliderShape.Rect   => GetRectBoundingBox(ref transform, ref collider, ref entity.Get<RectCollider>()),
        _                    => new FloatRect(transform.Position, Vector2.Zero),
    };

    private static FloatRect GetCircleBoundingBox(ref CircleCollider circle)
        => new(new(-circle.Radius), new(circle.Radius * 2f));

    private static FloatRect GetRectBoundingBox(ref Transform transform, ref Collider collider, ref RectCollider rect)
        => new(GetColliderOrigin(ref transform, ref collider) - (rect.Size / 2f), rect.Size);
}
