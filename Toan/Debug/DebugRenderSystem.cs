using System.Linq;

using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Systems;
using Toan.Physics;
using Toan.Rendering;

namespace Toan.Debug;

public class DebugRenderSystem : EntityRenderSystem
{
    public override WorldQuery<Debug, Transform> Archetype => new();

    public override void Render(World world, Renderer renderer, GameTime gameTime)
    protected override void RenderEntity(Entity entity, Renderer renderer, GameTime gameTime)
	{
        var debug = world.Resource<DebugState>();
        if (!debug.DebugActive) return;

		base.Render(world, renderer, gameTime);
	}

    protected override void RenderEntity(Entity entity, Renderer renderer, GameTime gameTime)
    {
		ref var transform = ref entity.Get<Transform>();
        if (entity.Has<Collider>())
        {
			bool hit = false;
			if (entity.Has<Collisions>())
			{
				hit = entity.Get<Collisions>().Any();
			}
			ref var collider = ref entity.Get<Collider>();
			switch (collider.Shape)
			{
				case ColliderShape.None:
					break;
				case ColliderShape.Circle:
					ref var circle = ref entity.Get<CircleCollider>();
                    renderer.DrawPrimitive(new DrawCircleCall
                    {
                        Color = hit ? Color.Red : Color.Yellow,
                        Radius = circle.Radius,
                        Position = transform.Position,
                        Rotation = MathUtil.DegToRad(transform.Rotation),
                        Scale = transform.Scale,
                        Sides = 32f,
                    });
					break;
				case ColliderShape.Rect:
					break;
			}
		}
	}
}
