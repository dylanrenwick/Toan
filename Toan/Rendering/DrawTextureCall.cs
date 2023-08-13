using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Toan.Rendering;

public record DrawTextureCall : DrawCall
{
    public required Texture2D Texture { get; init; }

    private Point TextureSize => new(Texture.Width, Texture.Height);
    private Vector2? _size;
    public Vector2 Size {
        get => _size ?? TextureSize.ToVector2();
        init => _size = value;
    }

    public Rectangle DestRect => new (
        location : MathUtil.RoundToPoint(Position),
        size     : MathUtil.RoundToPoint(Size * Scale)
    );

    public Vector2 SourcePosition { get; init; } = Vector2.Zero;
    public Vector2? SourceSize { get; init; }

    public Rectangle SourceRect => new(
        location : SourcePosition.ToPoint(),
        size     : SourceSize?.ToPoint() ?? TextureSize
    );
}
