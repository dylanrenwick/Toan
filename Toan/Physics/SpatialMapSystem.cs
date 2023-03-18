using System;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Systems;

namespace Toan.Physics;

public class SpatialMapSystem : PhysicsSystem
{
    public override WorldQuery<Collider, Changed<Transform>> Archetype => new();

    [UpdateSystem(SystemExecutionPriority.Late)]
    public override void Update(World world)
    {
        base.Update(world);

        if (_spatialMap != null)
        {
            foreach (Guid removedEntity in world.Events.Removed)
            {
                _spatialMap.Remove(removedEntity);
            }
        }
    }

    protected override void UpdateEntity(Entity entity)
    {
        if (_spatialMap == null)
            return;

        _spatialMap.Remove(entity.Id);
        _spatialMap.Add(entity);
    }
}
