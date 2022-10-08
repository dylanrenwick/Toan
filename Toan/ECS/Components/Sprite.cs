using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Toan.ECS.Components;

public class Sprite : GameComponent, ICloneable<Sprite>
{
    public required Texture2D Texture { get; set; }

    public Color Color { get; set; } = Color.White;

    public Vector2 Origin { get; set; } = Vector2.Zero;

    public Sprite Clone() => new()
    {
        Color = Color,
        Origin = Origin,
        Texture = Texture,
    };
}
