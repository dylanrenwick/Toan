using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Toan.Rendering;

public partial class Renderer
{
	public void DrawLine(DrawLineCall drawCall)
	{
		float distance = Vector2.Distance(drawCall.Position, drawCall.End);
		float angle = (drawCall.End - drawCall.Position).AngleRad();

		Vector2 cameraOffset = (MainCamera?.ViewOffset ?? Vector2.Zero) * ScreenSize;
		Vector2 drawOrigin = drawCall.Position * RenderScale + cameraOffset;

		Vector2 lineRect = new(distance, drawCall.StrokeWeight);

		_spriteBatch.Draw(
			texture         : Pixel,
			position        : drawOrigin,
			sourceRectangle : null,
			color           : drawCall.Color,
			rotation        : drawCall.Rotation + angle,
			origin          : drawCall.Origin,
			scale           : lineRect * RenderScale * drawCall.Scale,
			effects         : drawCall.SpriteEffects,
			layerDepth      : drawCall.LayerDepth
		);
	}

	public void DrawRect(DrawRectCall drawCall)
	{
		if (drawCall.Color == drawCall.FillColor)
			FillRectangle(drawCall);
		else DrawPrimitive(drawCall);
	}

	public void DrawPrimitive(DrawPrimitiveCall drawCall)
	{
		List<Vector2> points = drawCall.Points;

		for (int i = 0; i < points.Count; i++)
		{
			Vector2 currentPoint = points[i];
			Vector2 nextPoint = points[(i < points.Count - 1) ? i + 1 : 0];

			DrawLine(new(drawCall)
			{
				End      = nextPoint + drawCall.Position,
				Position = currentPoint + drawCall.Position,
			});
		}
	}

	private void FillRectangle(DrawRectCall drawCall)
	=> _spriteBatch.Draw(
		texture              : Pixel,
		destinationRectangle : ScaleAndOffsetRect(drawCall.Rect),
		sourceRectangle      : null,
		color                : drawCall.Color,
		rotation             : drawCall.Rotation,
		origin               : drawCall.Origin,
		effects              : drawCall.SpriteEffects,
		layerDepth           : drawCall.LayerDepth
	);
}
