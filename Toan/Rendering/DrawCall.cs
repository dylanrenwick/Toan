using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Toan.Rendering;

public abstract record DrawCall
{
    public required Vector2 Position { get; init; }

    public Color Color { get; init; } = Color.White;

    public float Rotation { get; init; } = 0f;
    public Vector2 Origin { get; init; } = Vector2.Zero;

    public Vector2 Scale { get; init; } = Vector2.One;

    public SpriteEffects SpriteEffects { get; init; } = SpriteEffects.None;

    public float LayerDepth { get; init; } = 0f;

	public DrawCall() { }
	public DrawCall(DrawCall other)
	{
		Position = other.Position;
		Color = other.Color;
		Rotation = other.Rotation;
		Origin = other.Origin;
		Scale = other.Scale;
		SpriteEffects = other.SpriteEffects;
		LayerDepth = other.LayerDepth;
	}
}
