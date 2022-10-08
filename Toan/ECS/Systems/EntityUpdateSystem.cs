using Microsoft.Xna.Framework;

using Toan.ECS.Resources;

namespace Toan.ECS.Systems;
public abstract class EntityUpdateSystem : EntitySystem, IUpdatable
{
#if DEBUG
    protected TextLog? Debug { get; private set; }
#endif

    public virtual void Update(World world, GameTime gameTime)
    {
#if DEBUG
        Debug = world.Resource<TextLog>();
#endif

        foreach (var entityId in _entities)
        {
            UpdateEntity(
                new() {
                    World      = world,
                    Id         = entityId,
                    Components = world.Components(entityId),
                },
                gameTime
            );
        }
		_isDirty = false;
    }

    protected abstract void UpdateEntity(Entity entity, GameTime gameTime);
}

