using Microsoft.Xna.Framework;

using Toan.ECS.Resources;

namespace Toan.ECS.Systems;
public abstract class EntityUpdateSystem : EntitySystem
{
#if DEBUG
    protected TextLog? Debug { get; private set; }
#endif

    [UpdateSystem]
    public virtual void Update(World world, GameTime gameTime)
    {
#if DEBUG
        Debug = world.Resource<TextLog>();
#endif

        foreach (var entityId in _entities)
        {
            UpdateEntity(
                world.Entity(entityId),
                gameTime
            );
        }
		_isDirty = false;
    }

    protected abstract void UpdateEntity(Entity entity, GameTime gameTime);
}

