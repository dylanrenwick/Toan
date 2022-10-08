using System;
using System.Collections.Generic;

namespace Toan.ECS.Systems;

public abstract class EntitySystem : IGameSystem, IEntitySystem
{
    public abstract IQuery Archetype { get; }

    protected HashSet<Guid> _entities = new();

	protected bool _isDirty = false;

    public virtual void UpdateComponents(World _, IReadOnlySet<Guid> entities)
    {
		_isDirty = true;
        _entities.Clear();
        _entities.UnionWith(entities);
    }
}

