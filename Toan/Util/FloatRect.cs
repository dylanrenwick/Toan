using System;

using Microsoft.Xna.Framework;

namespace Toan;

public struct FloatRect
{
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }

    public float Left => Math.Min(Position.X, Position.X + Size.X);
    public float Right => Math.Max(Position.X, Position.X + Size.X);
    public float Top => Math.Min(Position.Y, Position.Y + Size.Y);
    public float Bottom => Math.Max(Position.Y, Position.Y + Size.Y);

    public float Width => Right - Left;
    public float Height => Bottom - Top;

    public float OuterBoundsRadius => (Size / 2f).Length();
    public float InnerBoundsRadius => Math.Min(Size.X, Size.Y) / 2f;

    public FloatRect(Vector2 pos, Vector2 size)
    {
        Position = pos;
        Size = size;
    }

    public FloatRect(float x, float y, float w, float h)
    {
        Position = new(x, y);
        Size = new(w, h);
    }

    public FloatRect()
    {
        Position = Vector2.Zero;
        Size = Vector2.Zero;
    }

    public bool Contains(Vector2 point)
        => point.X > Left && point.X < Right
            && point.Y > Bottom && point.Y < Top;

    public FloatRect Offset(Vector2 offset)
        => new FloatRect(Position + offset, Size);

    public bool Overlaps(FloatRect other)
    {
        return Right > other.Left && Left < other.Right
            && Top > other.Bottom && Bottom < other.Top;
    }

    public Rectangle ToRectangle()
        => new(Position.ToPoint(), Size.ToPoint());
}
