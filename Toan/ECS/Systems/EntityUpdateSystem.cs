using Toan.ECS.Resources;

namespace Toan.ECS.Systems;
public abstract class EntityUpdateSystem : EntitySystem
{
#if DEBUG
    protected TextLog? Debug { get; private set; }
#endif

    [UpdateSystem]
    public virtual void Update(World world)
    {
#if DEBUG
        Debug = world.Resource<TextLog>();
#endif

        foreach (var entityId in _entities)
        {
            if (!world.HasEntity(entityId)) continue;
            UpdateEntity(
                world.Entity(entityId)
            );
        }
    }

    protected abstract void UpdateEntity(Entity entity);
}

