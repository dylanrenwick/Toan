using System;

using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;

namespace Toan.Physics;

public static class CollisionHelper
{
    public static bool CheckRectPointCollision(Entity rect, Entity point)
        => CheckRectPointCollision(rect, GetColliderOrigin(point));

    public static bool CheckRectPointCollision(Entity rect, Vector2 point)
        => GetColliderBoundingBox(rect)
            .Contains(point);

    public static bool CheckCircleRectCollision(Entity circle, Entity rect)
    {
        var circleOrigin = GetColliderOrigin(circle);
        var rectOrigin   = GetColliderOrigin(rect);

        var rectBox = GetColliderBoundingBox(rect);

        var circleRadius = GetColliderBoundingCircle(circle);
        var rectInnerRadius = rectBox.InnerBoundsRadius;
        if (!CheckCircleCircleCollision(circleOrigin, circleRadius, rectOrigin, rectInnerRadius))
            return false;

        var diffLine = rectOrigin - circleOrigin;
        diffLine.Normalize();

        var contactPoint = diffLine * circleRadius;
        return CheckRectPointCollision(rect, contactPoint);
    }

    public static bool CheckCircleCircleCollision(Entity entityA, Entity entityB)
    => CheckCircleCircleCollision(
        originA: GetColliderOrigin(entityA),
        radiusA: GetColliderBoundingCircle(entityA),
        originB: GetColliderOrigin(entityB),
        radiusB: GetColliderBoundingCircle(entityB)
    );

    public static bool CheckCircleCircleCollision(Vector2 originA, float radiusA, Vector2 originB, float radiusB)
        => (originB - originA).LengthSquared() < Math.Pow(radiusA + radiusB, 2);

    public static bool CheckRectRectCollision(Entity entityA, Entity entityB)
    => CheckRectRectCollision(
        rectA: GetColliderBoundingBox(entityA),
        rectB: GetColliderBoundingBox(entityB)
    );

    public static bool CheckRectRectCollision(FloatRect rectA, FloatRect rectB)
        => rectA.Overlaps(rectB);

    public static Vector2 GetColliderOrigin(Entity entity)
        => GetColliderOrigin(entity.Get<Transform>(), entity.Get<Collider>());

    public static Vector2 GetColliderOrigin(Transform transform, Collider collider)
        => transform.Position + collider.Origin;

    public static float GetColliderBoundingCircle(Entity entity)
        => GetColliderBoundingCircle(entity, entity.Get<Collider>());

    public static float GetColliderBoundingCircle(Entity entity, Collider collider)
    => collider.Shape switch
    {
        ColliderShape.Circle => entity.Get<CircleCollider>().Radius,
        ColliderShape.Rect   => GetColliderBoundingBox(entity, collider).OuterBoundsRadius,
        _                    => 0f,
    };

    /// <summary>
    /// Utility method for calculating smallest AABB containing collider
    /// </summary>
    /// <param name="entity">The entity whos collider to use</param>
    /// <returns></returns>
    public static FloatRect GetColliderBoundingBox(Entity entity)
        => GetColliderBoundingBox(entity, entity.Get<Collider>());
    /// <summary>
    /// Utility method for calculating smallest AABB containing collider
    /// </summary>
    /// <param name="entity">The entity whos collider to use</param>
    /// <param name="collider">A reference to the <see cref="Collider"/> component</param>
    /// <returns></returns>
    public static FloatRect GetColliderBoundingBox(Entity entity, Collider collider)
        => GetColliderBoundingBox(entity, collider, entity.Get<Transform>());
    /// <summary>
    /// Utility method for calculating smallest AABB containing collider
    /// </summary>
    /// <param name="entity">The entity whos collider to use</param>
    /// <param name="collider">A reference to the <see cref="Collider"/> component</param>
    /// <param name="transform">A reference to the <see cref="Transform"/> component</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public static FloatRect GetColliderBoundingBox(Entity entity, Collider collider, Transform transform)
    => collider.Shape switch
    {
        ColliderShape.Circle => GetCircleBoundingBox(entity.Get<CircleCollider>()).Offset(transform.Position),
        ColliderShape.Rect   => GetRectBoundingBox(transform, collider, entity.Get<RectCollider>()),
        _                    => new FloatRect(transform.Position + collider.Origin, Vector2.Zero),
    };

    private static FloatRect GetCircleBoundingBox(CircleCollider circle)
        => new(new(-circle.Radius), new(circle.Radius * 2f));

    private static FloatRect GetRectBoundingBox(Transform transform, Collider collider, RectCollider rect)
        => new(GetColliderOrigin(transform, collider) - (rect.Size / 2f), rect.Size);
}
