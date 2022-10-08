using Microsoft.Xna.Framework;

using Toan.Rendering;

namespace Toan.ECS.Systems;

public abstract class EntityRenderSystem : EntitySystem, IRenderable
{
    public virtual void Render(World world, Renderer renderer, GameTime gameTime)
    {
        foreach (var entityId in _entities)
        {
            RenderEntity(
                new()
                {
                    World      = world,
                    Id         = entityId,
                    Components = world.Components(entityId),
                },
                renderer,
                gameTime
            );
        }
		_isDirty = false;
    }

    protected abstract void RenderEntity(Entity entity, Renderer renderer, GameTime gameTime);
}

