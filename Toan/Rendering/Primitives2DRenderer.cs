using Microsoft.Xna.Framework;

namespace Toan.Rendering;

public partial class Renderer
{
	public void DrawLine(DrawLineCall drawCall)
	{
		float distance = Vector2.Distance(drawCall.Position, drawCall.End);
		float angle = (drawCall.End - drawCall.Position).AngleRad();

		Vector2 lineRect = new(distance, drawCall.StrokeWeight);

		_spriteBatch.Draw(
			texture         : Pixel,
			position        : drawCall.Position,
			sourceRectangle : null,
			color           : drawCall.Color,
			rotation        : drawCall.Rotation + angle,
			origin          : drawCall.Origin,
			scale           : lineRect * drawCall.Scale,
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
		var points = drawCall.Points;
		var cameraOffset = (MainCamera?.ViewOffset ?? Vector2.Zero) * ScreenSize;
		var drawOrigin = drawCall.Position * RenderScale;

		var drawOffset = drawOrigin + cameraOffset;

		for (int i = 0; i < points.Count; i++)
		{
			var currentPoint = points[i] + drawOffset;
			var nextPoint = points[(i < points.Count - 1) ? i + 1 : 0] + drawOffset;

			DrawLine(new(drawCall)
			{
				End      = nextPoint,
				Position = currentPoint,
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
