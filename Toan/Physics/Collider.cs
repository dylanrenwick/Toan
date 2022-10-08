using Microsoft.Xna.Framework;

using GameComponent = Toan.ECS.Components.GameComponent;

namespace Toan.Physics;

public abstract class Collider : GameComponent
{
    public required Vector2 Origin { get; set; }
}
