using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Toan.Rendering;

public record DrawCircleCall : DrawPrimitiveCall
{
	private static readonly Dictionary<(float, float), List<Vector2>> _circleCache = new();

	public required float Radius { get; init; }
	public required float Sides { get; init; }

	public override List<Vector2> Points
	{
		get
		{
			if (_circleCache.TryGetValue((Radius, Sides), out List<Vector2>? cachedPoints))
			{
				return cachedPoints;
			}
			else
			{
				var circlePoints = MathUtil.GetCirclePoints(Radius, Sides);
				_circleCache.Add((Radius, Sides), circlePoints);
				return circlePoints;
			}
		}
	}

	public DrawCircleCall() : base() { }
	public DrawCircleCall(DrawCircleCall other) : base(other)
	{
		Radius = other.Radius;
		Sides = other.Sides;
	}
	public DrawCircleCall(DrawPrimitiveCall other) : base(other) { }
}
