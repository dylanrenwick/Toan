using Microsoft.Xna.Framework;

using GameComponent = Toan.ECS.Components.GameComponent;

namespace Toan.Physics;

public abstract class Collider : GameComponent
{
    public required Vector2 Origin { get; set; }

    public required CollisionMask Mask { get; set; }
    
    public required ulong Layer { get; set; }
}
