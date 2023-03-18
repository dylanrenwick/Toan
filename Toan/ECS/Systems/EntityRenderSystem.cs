using Microsoft.Xna.Framework;

using Toan.Rendering;

namespace Toan.ECS.Systems;

public abstract class EntityRenderSystem : EntitySystem
{
    [RenderSystem]
    public virtual void Render(World world, Renderer renderer)
    {
        foreach (var entityId in _entities)
        {
            if (!world.HasEntity(entityId)) continue;
            RenderEntity(
                world.Entity(entityId),
                renderer
            );
        }
    }

    protected abstract void RenderEntity(Entity entity, Renderer renderer);
}

