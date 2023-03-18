using System;
using System.Collections.Generic;
using Toan.ECS.Query;

namespace Toan.ECS.Systems;

public abstract class EntitySystem
{
    public abstract IWorldQuery Archetype { get; }

    protected HashSet<Guid> _entities = new();

    [EntitySystem("Archetype")]
    public virtual void UpdateComponents(World _, IReadOnlySet<Guid> entities)
    {
        _entities.Clear();
        _entities.UnionWith(entities);
    }
}

