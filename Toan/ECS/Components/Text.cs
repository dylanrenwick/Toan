using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Toan.ECS.Components;

public class Text : GameComponent, ICloneable<Text>
{
    public required SpriteFont Font { get; set; }

    public string Content { get; set; } = string.Empty;
    public Color Color { get; set; } = Color.White;

    public Text Clone() => new()
    {
        Color = Color,
        Content = Content,
        Font = Font,
    };
}
