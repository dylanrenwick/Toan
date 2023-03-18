using Toan.ECS;
using Toan.ECS.Systems;

namespace Toan.Physics;

public abstract class PhysicsSystem : EntityUpdateSystem
{
    protected SpatialMap? _spatialMap;

    [UpdateSystem]
    public override void Update(World world)
    {
        _spatialMap = world.Resource<SpatialMap>();

        if (_spatialMap != null)
            base.Update(world);
    }
}

