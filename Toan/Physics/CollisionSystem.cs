using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;

namespace Toan.Physics;
public class CollisionSystem : PhysicsSystem
{
    public override WorldQuery<Collider, Transform> Archetype => new();

    private struct Collidable
    {
        public Entity Entity;

        public Collider Collider => Entity.Get<Collider>();
        public Transform Transform => Entity.Get<Transform>();

        public FloatRect GetColliderBoundingBox()
            => CollisionHelper.GetColliderBoundingBox(Entity, Collider, Transform);

        public static Collidable FromEntity(Entity entity)
        => new()
        {
            Entity = entity,
        };
    }

    protected override void UpdateEntity(Entity entity, GameTime gameTime)
    {
        if (_spatialMap is null)
            return;

        var entityCollidable = Collidable.FromEntity(entity);

        FloatRect boundingBox = entityCollidable.GetColliderBoundingBox();
        IReadOnlySet<Guid> nearbyColliders = _spatialMap.GetPossibleCollisions(boundingBox);

        Collisions collisions = new();

        foreach (Guid otherId in nearbyColliders)
        {
            if (otherId == entity.Id)
                continue;

            var other = entity.World.Entity(otherId);
            var otherCollidable = Collidable.FromEntity(other);

            Vector2? collision = CheckCollisions(entityCollidable, otherCollidable);
            if (collision is Vector2 collisionNormal)
            {
                collisions.Add(new()
                {
                    CollisionNormal = collisionNormal,
                    Other           = otherId,
                });
            }
        }

        if (collisions.Count > 0)
            entity.With(collisions);
        else
            entity.Without<Collisions>();
    }

    private static Vector2? CheckCollisions(Collidable first, Collidable second)
    {
        Vector2 collisionNormal = second.Transform.Position - first.Transform.Position;

        // Check circle collisions first
        CollisionResult circleResult = CheckCircleCollisions(first, second);
        // Exit early if circle collision fails
        if (circleResult == CollisionResult.None)
            return null;
        else if (circleResult == CollisionResult.Full)
            return collisionNormal;

        CollisionResult rectResult = CheckRectCollisions(first, second);
        if (rectResult == CollisionResult.None)
            return null;
        else if (rectResult == CollisionResult.Full)
            return collisionNormal;

        return null;
    }

    private enum CollisionResult
    {
        None,
        Partial,
        Full
    }

    private static CollisionResult CheckCircleCollisions(Collidable first, Collidable second)
    {
        // If circle collision fails
        if (!CollisionHelper.CheckCircleCircleCollision(first.Entity, second.Entity))
            return CollisionResult.None;
        // If circle collision passes and both colliders are circle
        else if (first.Collider.IsCircle && second.Collider.IsCircle)
            return CollisionResult.Full;
        // If circle collision passes and first is circle
        else if (first.Collider.IsCircle)
            return CheckCircleNonCircleCollisions(first, second);
        // If circle collision passes and second is circle
        else if (second.Collider.IsCircle)
            return CheckCircleNonCircleCollisions(second, first);
        // If circle collision passes but neither collider is circle
        else
            return CollisionResult.Partial;
    }

    private static CollisionResult CheckCircleNonCircleCollisions(Collidable circle, Collidable nonCircle)
    => nonCircle.Collider.Shape switch
    {
        ColliderShape.Rect    => CollisionHelper.CheckCircleRectCollision(circle.Entity, nonCircle.Entity)
            ? CollisionResult.Full
            : CollisionResult.None,
        ColliderShape.Circle
        | ColliderShape.Point => throw new UnreachableException(),
        _                     => CollisionResult.None,
    };

    private static CollisionResult CheckRectCollisions(Collidable first, Collidable second)
    {
        // If rect collision fails
        if (!CollisionHelper.CheckRectRectCollision(first.Entity, second.Entity))
            return CollisionResult.None;
        // If rect collision passes and both colliders are rect
        else if (first.Collider.IsRect && second.Collider.IsRect)
            return CollisionResult.Full;
        // If rect collision passes and first is rect
        else if (first.Collider.IsRect)
            return CheckRectNonRectCollisions(first, second);
        // If rect collision passes and second is rect
        else if (second.Collider.IsRect)
            return CheckRectNonRectCollisions(second, first);
        // If rect collision passes but neither collider is rect
        else
            return CollisionResult.Partial;
    }

    private static CollisionResult CheckRectNonRectCollisions(Collidable rect, Collidable nonRect)
    => nonRect.Collider.Shape switch
    {
        ColliderShape.Circle
        | ColliderShape.Point
        | ColliderShape.Rect  => throw new UnreachableException(),
        _                     => CollisionResult.None,
    };
}

