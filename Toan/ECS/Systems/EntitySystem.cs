﻿using System;
using System.Collections.Generic;
using Toan.ECS.Query;

namespace Toan.ECS.Systems;

public abstract class EntitySystem : IGameSystem, IEntitySystem
{
    public abstract IWorldQuery Archetype { get; }

    protected HashSet<Guid> _entities = new();

	protected bool _isDirty = false;

    public virtual void UpdateComponents(World _, IReadOnlySet<Guid> entities)
    {
		_isDirty = true;
        _entities.Clear();
        _entities.UnionWith(entities);
    }
}

