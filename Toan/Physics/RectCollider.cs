using Microsoft.Xna.Framework;

using Toan.ECS.Components;

namespace Toan.Physics;

public struct RectCollider : IComponent
{
    public required Vector2 Size { get; set; }
}
