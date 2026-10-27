using Microsoft.Xna.Framework;

namespace Toan.Test.Physics;

public class Geometry_Tests
{
	[Fact]
	public void Rectangle_ContainsCenter()
	{
		FloatRect rect = new(0f, 5f, 10.5f, 10f / 3f);
		Vector2 center = new(5.75f, 6.66f);

		Assert.True(rect.Contains(center));
	}

	[Theory]
	[InlineData(1f, 6f)]
	[InlineData(10f, 8.33f)]
	public void Rectangle_ContainsInner(float x, float y)
	{
		FloatRect rect = new(0f, 5f, 10.5f, 10f / 3f);
		Vector2 point = new(x, y);

		Assert.True(rect.Contains(point));
	}

	[Theory]
	[InlineData(1f, 0f)]
	[InlineData(1f, 10f)]
	[InlineData(-1f, 6f)]
	[InlineData(12f, 6f)]
	[InlineData(-1f, 0f)]
	[InlineData(12f, 10f)]
	public void Rectangle_DoesntContainOuter(float x, float y)
	{
		FloatRect rect = new(0f, 5f, 10.5f, 10f / 3f);
		Vector2 point = new(x, y);

		Assert.False(rect.Contains(point));
	}

	[Fact]
	public void Rectangle_OverlapsSelf()
	{
		FloatRect rect = new(0f, 5f, 10.5f, 10f / 3f);

		Assert.True(rect.Overlaps(rect));
	}

	[Theory]
	[InlineData(1f, 6f, 1f, 1f)] // Entirely within
	[InlineData(-1f, 6f, 10f, 3f)] // Overlaps left
	[InlineData(8f, 6f, 5f, 3f)] // Overlaps right
	[InlineData(1f, 4f, 1f, 3f)] // Overlaps top
	[InlineData(1f, 8f, 1f, 3f)] // Overlaps bottom
	[InlineData(-1f, 4f, 12f, 5f)] // Entirely contains
	[InlineData(1f, 7f, 1f, -3f)] // Overlaps top, negative size
	public void Rectangle_OverlapsInner(float x, float y, float w, float h)
	{
		FloatRect rect = new(0f, 5f, 10.5f, 10f / 3f);
		FloatRect other = new(x, y, w, h);

		Assert.True(rect.Overlaps(other));
	}

	[Theory]
	[InlineData(-2f, 6f, 1f, 1f)] // Too far left, vertical aligned
	[InlineData(11f, 6f, 1f, 1f)] // Too far right, vertical aligned
	[InlineData(2f, 1f, 1f, 1f)] // Too far up, horizontal aligned
	[InlineData(2f, 10f, 1f, 1f)] // Too far down, horizontal aligned
	[InlineData(-2f, 1f, 1f, 1f)] // Too far left and up
	[InlineData(-2f, 10f, 1f, 1f)] // Too far left and down
	[InlineData(11f, 1f, 1f, 1f)] // Too far right and up
	[InlineData(11f, 10f, 1f, 1f)] // Too far right and down
	public void Rectangle_DoesntOverlapOuter(float x, float y, float w, float h)
	{
		FloatRect rect = new(0f, 5f, 10.5f, 10f / 3f);
		FloatRect other = new(x, y, w, h);

		Assert.False(rect.Overlaps(other));
	}
}
