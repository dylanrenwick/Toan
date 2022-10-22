﻿using System;
using System.Collections.Generic;
using System.Linq;
using Toan.ECS.Query;

namespace Toan.ECS.Components;

public abstract class GameComponent : IWorldQueryable
{
    public Guid Id { get; } = Guid.NewGuid();

    public ISet<Guid> Reduce(ISet<Guid> entities, World world)
    {
        Type typeToFind = GetType();

        return entities
            .Where(entity => world.Entity(entity).Components.Has(typeToFind))
            .ToHashSet();
    }
}
