using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Toan.Rendering;

public record DrawPrimitiveCall : DrawCall
{
	public virtual List<Vector2> Points { get; init; } = new();
	public Color? FillColor { get; init; }
	public float StrokeWeight { get; init; } = 1f;

	public DrawPrimitiveCall() : base() { }
	public DrawPrimitiveCall(DrawPrimitiveCall other) : base(other)
	{
		Points = other.Points;
		FillColor = other.FillColor;
		StrokeWeight = other.StrokeWeight;
	}
}
