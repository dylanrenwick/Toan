using System;
using System.Collections.Generic;
using System.Linq;

namespace Toan.ECS.Components;

public abstract class GameComponent : IWorldQueryable
{
    public Guid Id { get; } = Guid.NewGuid();

    public ISet<Guid> Reduce(ISet<Guid> entities, World world)
    {
        Type typeToFind = GetType();

        return entities
            .Where(entity => world.Entities[entity].Has(typeToFind))
            .ToHashSet();
    }
}
