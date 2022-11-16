using Toan.ECS.Components;

namespace Toan.Physics;

public struct CircleCollider : IComponent
{
    public required float Radius { get; set; }
}
