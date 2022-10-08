using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Toan.Rendering;

public record DrawTextureCall : DrawCall
{
    public required Texture2D Texture { get; init; }

    private Point TextureSize => new(Texture.Width, Texture.Height);
    public Vector2? Size { get; init; }

    public Rectangle DestRect => new (
        location: Position.ToPoint(),
        size: ((Size ?? TextureSize.ToVector2()) * Scale).ToPoint()
    );

    public Vector2 SourcePosition { get; init; } = Vector2.Zero;
    public Vector2? SourceSize { get; init; }

    public Rectangle SourceRect => new(
        location: SourcePosition.ToPoint(),
        size: SourceSize?.ToPoint() ?? TextureSize
    );
}
