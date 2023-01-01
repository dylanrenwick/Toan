using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Toan.ECS.Components;

public struct Text : IComponent, ICloneable<Text>
{
    public required SpriteFont Font { get; set; }

    public string Content { get; set; } = string.Empty;
    public Color Color { get; set; } = Color.White;

    public Text() { }

    public Text Clone() => new()
    {
        Color = Color,
        Content = Content,
        Font = Font,
    };
}
