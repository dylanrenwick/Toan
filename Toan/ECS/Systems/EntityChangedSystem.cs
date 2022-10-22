using System;
using System.Collections.Generic;

namespace Toan.ECS.Systems;

public abstract class EntityChangedSystem : IGameSystem, IEntitySystem
{
    public abstract IWorldQuery Archetype { get; }

    protected HashSet<Guid> _entities = new();

    public void UpdateComponents(World world, IReadOnlySet<Guid> entities)
    {
        _entities.Clear();
        _entities.UnionWith(entities);

		EntitiesChanged(world);
    }

	protected virtual void EntitiesChanged(World world)
	{
		foreach (var entityId in _entities)
		{
			EntityChanged(new()
			{
				World      = world,
				Id         = entityId,
				Components = world.Components(entityId),
			});
		}
	}

	protected abstract void EntityChanged(Entity entity);
}
