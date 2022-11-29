using System;
using System.Linq;

using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Query;

namespace Toan.Physics;

public class SpatialMapSystem : PhysicsSystem
{
    public override WorldQuery<Collider, Added> Archetype => new();

    public override void Update(World world, GameTime gameTime)
    {
        base.Update(world, gameTime);

        if (_spatialMap != null)
        {
            foreach (Guid removedEntity in world.Events.Removed)
            {
                _spatialMap.Remove(removedEntity);
            }
        }
    }

    protected override void UpdateEntity(Entity entity, GameTime gameTime)
    {
        if (_spatialMap == null)
            return;

        _spatialMap.Add(entity);
    }
}
