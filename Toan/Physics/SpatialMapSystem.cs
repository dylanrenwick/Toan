using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Query;

namespace Toan.Physics;

public class SpatialMapSystem : PhysicsSystem
{
    public override WorldQuery<Collider, Added> Archetype => new();

    protected override void UpdateEntity(Entity entity, GameTime gameTime)
    {
        if (_spatialMap == null)
            return;

        _spatialMap.Add(entity);
    }
}
