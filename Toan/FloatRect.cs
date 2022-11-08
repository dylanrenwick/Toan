using System;

using Microsoft.Xna.Framework;

namespace Toan;

public struct FloatRect
{
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }

    public float Left   => Math.Min(Position.X, Position.X + Size.X);
    public float Right  => Math.Max(Position.X, Position.X + Size.X);
    public float Top    => Math.Min(Position.Y, Position.Y + Size.Y);
    public float Bottom => Math.Max(Position.Y, Position.Y + Size.Y);

    public float Width  => Right - Left;
    public float Height => Bottom - Top;

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

    public FloatRect Offset(Vector2 offset)
        => new FloatRect(Position + offset, Size);
}
