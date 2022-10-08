using Microsoft.Xna.Framework;

using Toan.ECS;

namespace Toan.Physics;

public interface IPhysicsUpdateable
{
    public void PhysicsUpdate(World scene, GameTime time);
}

