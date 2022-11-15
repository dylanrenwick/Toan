using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Systems;

namespace Toan.Physics;
public class CollisionSystem : EntityUpdateSystem
{
    public override WorldQuery<Collider, Transform> Archetype => new();

    private SpatialMap? spatialMap;

    private ref struct Collidable
    {
        public Entity Entity;

        public ref Collider Collider;
        public ref Transform Transform;

        public FloatRect GetColliderBoundingBox()
            => Collider.GetColliderBoundingBox(Entity, ref Collider, ref Transform);

        public static Collidable FromEntity(Entity entity)
        => new()
        {
            Entity = entity,
            Collider = entity.Get<Collider>(),
            Transform = entity.Get<Transform>(),
        };
    }

    public override void Update(World world, GameTime gameTime)
    {
        spatialMap = world.Resource<SpatialMap>();

        base.Update(world, gameTime);
    }

    protected override void UpdateEntity(Entity entity, GameTime gameTime)
    {
        if (spatialMap is null)
            return;

        var entityCollidable = Collidable.FromEntity(entity);

        FloatRect boundingBox = entityCollidable.GetColliderBoundingBox();
        IReadOnlySet<Guid> nearbyColliders = spatialMap.GetPossibleCollisions(boundingBox);

        Collisions collisions = new();

        foreach (Guid otherId in nearbyColliders)
        {
            var other = entity.World.Entity(otherId);
            var otherCollidable = Collidable.FromEntity(other);

            Vector2? collision = CheckCollisions(entityCollidable, otherCollidable);
            if (collision is Vector2 collisionNormal)
            {
                // Handle found collision
            }
        }

        entity.Without<Collisions>();

        if (collisions.Count > 0)
        {
            entity.With(collisions);
        }
    }

    private Vector2? CheckCollisions(Collidable first, Collidable second)
    {
        Vector2 collisionNormal = second.Transform.Position - first.Transform.Position;

        FloatRect firstBoundingBox  = first.GetColliderBoundingBox();
        FloatRect secondBoundingBox = second.GetColliderBoundingBox();

        // If containing AABBs do not overlap, colliders cannot intersect
        if (!firstBoundingBox.Overlaps(secondBoundingBox))
            return null;
        // If both colliders are AABBs, we just did the collision check
        if (first.Collider.Shape == ColliderShape.Rect && second.Collider.Shape == ColliderShape.Rect)
            return collisionNormal;


    }
}

