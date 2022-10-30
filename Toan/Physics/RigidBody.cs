using Toan.ECS.Components;

namespace Toan.Physics;

public struct RigidBody : IComponent
{
    public required Motor Motor;
    public required Collider Collider;
}

