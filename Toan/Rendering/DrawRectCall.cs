using System;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Toan.Rendering;

public record DrawRectCall : DrawPrimitiveCall
{
	public required Rectangle Rect
	{
		get
		{
			if (Points.Count != 4) throw new InvalidOperationException("Primitive is not a rectangle");
			var location = Points.First();
			var opposite = Points[2];
			var size = opposite - location;
			return new(location.ToPoint(), size.ToPoint());
		}
		init
		{
			Points.Clear();
			Points.Add(value.Location.ToVector2());
			Points.Add(value.Location.ToVector2() + new Vector2(value.Size.X, 0));
			Points.Add(value.Location.ToVector2() + value.Size.ToVector2());
			Points.Add(value.Location.ToVector2() + new Vector2(0, value.Size.Y));
		}
	}

	public DrawRectCall() : base() { }
	public DrawRectCall(DrawRectCall other) : base(other)
	{
		Rect = other.Rect;
	}
	public DrawRectCall(DrawPrimitiveCall other) : base(other) { }
}

