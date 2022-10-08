using Microsoft.Xna.Framework;

using Toan.ECS.Components;
using Toan.ECS;
using GameComponent = Toan.ECS.Components.GameComponent;

namespace Toan.Physics;

public class RigidBody : GameComponent, IPhysicsUpdateable
{
    public required Motor Motor;
    public required Collider Collider;

    public void PhysicsUpdate(World scene, GameTime time)
    {

    }
}

