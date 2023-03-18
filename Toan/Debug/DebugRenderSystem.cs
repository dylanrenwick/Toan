using System.Linq;

using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Systems;
using Toan.Input;
using Toan.Physics;
using Toan.Rendering;

namespace Toan.Debug;

public class DebugRenderSystem : EntityRenderSystem
{
    public override WorldQuery<Debug, Transform> Archetype => new();

	[RenderSystem]
    public override void Render(World world, Renderer renderer, GameTime gameTime)
	{
        var debug = world.Resource<DebugState>();
        if (!debug.ShouldDisplay) return;

		var spatialMap = world.Resource<SpatialMap>();
		DrawSpatialMap(renderer, spatialMap);

        InputState input = world.Resource<InputState>();
        Camera camera = MainCamera.MainCameraEntity;
        Vector2 mouseWorldPos = camera.ScreenToWorld(input.MousePosition.ToVector2());
		renderer.DrawPrimitive(new DrawCircleCall
		{
			Color = Color.LightBlue,
			Radius = 4f,
			Position = mouseWorldPos,
			Sides = 32f,
		});

		base.Render(world, renderer, gameTime);
	}

    protected override void RenderEntity(Entity entity, Renderer renderer, GameTime gameTime)
    {
		var transform = entity.Get<Transform>();
        if (entity.Has<Collider>())
        {
			bool hit = false;
			if (entity.Has<Collisions>())
			{
				hit = entity.Get<Collisions>().Count == 0;
			}
			var collider = entity.Get<Collider>();
			switch (collider.Shape)
			{
				case ColliderShape.None:
					break;
				case ColliderShape.Circle:
					var circle = entity.Get<CircleCollider>();
                    renderer.DrawPrimitive(new DrawCircleCall
                    {
                        Color = hit ? Color.Red : Color.Yellow,
                        Radius = circle.Radius,
                        Position = transform.Position + collider.Origin,
                        Rotation = MathUtil.DegToRad(transform.Rotation),
                        Scale = transform.Scale,
                        Sides = 32f,
                    });
					break;
				case ColliderShape.Rect:
					break;
			}

			FloatRect boundingBox = CollisionHelper.GetColliderBoundingBox(entity, collider, transform);
			renderer.DrawRect(new()
			{
                Color = hit ? Color.Red : Color.Yellow,
				Position = boundingBox.Position,
				Rect = new(new(0), boundingBox.Size.ToPoint())
			});
		}
	}

	private static void DrawSpatialMap(Renderer renderer, SpatialMap spatialMap)
	{
		Point cellSize = new(spatialMap.CellSize);

		foreach (Point cell in spatialMap.OccupiedCells)
		{
			var cellContents = spatialMap[cell.X, cell.Y];
			if (cellContents.Count == 0)
				continue;

			var cellPos = cell * cellSize;

			renderer.DrawRect(new()
			{
				Color = Color.Cyan,
				Position = cellPos.ToVector2(),
				Rect = new(new(0), cellSize),
				StrokeWeight = 1f / renderer.RenderScale,
			});
		}
	}
}
