using Toan.ECS;

namespace Toan.Physics;

public class PhysicsPlugin : Plugin
{
    public override void Build(World world)
    {
        world.AddSystem<CircleCollisionSystem>();
    }
}
