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

    protected SpatialMap? spatialMap;

    public override void Update(World world, GameTime gameTime)
    {
        spatialMap = world.Resource<SpatialMap>();

        base.Update(world, gameTime);
    }

    protected override void UpdateEntity(Entity entity, GameTime gameTime)
    {
        if (spatialMap is null)
            return;

        ref var collider  = ref entity.Get<Collider>();
        ref var transform = ref entity.Get<Transform>();

        FloatRect boundingBox = Collider.GetColliderBoundingBox(entity, ref collider, ref transform);
        IReadOnlySet<Guid> nearbyColliders = spatialMap.GetPossibleCollisions(boundingBox);

        Collisions collisions = new();

        foreach (Guid otherId in nearbyColliders)
        {
            var other = entity.World.Entity(otherId);
            // Check if entity collides with other            
        }

        entity.Without<Collisions>();

        if (collisions.Count > 0)
        {
            entity.With(collisions);
        }
    }
}

