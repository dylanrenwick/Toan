using Toan.ECS;

namespace Toan.Physics;

public class PhysicsPlugin : Plugin
{
    public int SpatialCellSize { get; init; } = 8;

    public override void Build(World world)
    {
        world.AddResource(new SpatialMap { CellSize = SpatialCellSize });

        world.AddSystem<CollisionSystem>();
    }
}
