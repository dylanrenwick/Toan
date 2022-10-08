using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Toan.Rendering;

public record DrawLineCall : DrawPrimitiveCall
{
	public required Vector2 End { get; init; }

	public override List<Vector2> Points => new() { Position, End };

	public DrawLineCall() : base() { }
	public DrawLineCall(DrawLineCall other) : base(other)
	{
		End = other.End;
	}
	public DrawLineCall(DrawPrimitiveCall other) : base(other) { }
}
