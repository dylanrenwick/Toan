using Microsoft.Xna.Framework.Graphics;

namespace Toan.Rendering;

public record DrawStringCall : DrawCall
{
    public required SpriteFont Font { get; init; }
    public required string Text { get; init; }
}
