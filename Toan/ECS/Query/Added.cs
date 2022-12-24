using System;
using System.Collections.Generic;
using System.Linq;

using Toan.ECS.Components;

namespace Toan.ECS.Query;

/// <summary>
/// Indicates that the query should only match entities added to the world since the last update.
/// </summary>
public class Added : IWorldQueryable
{
    public ISet<Guid> Reduce(World world, ISet<Guid> entities, ComponentRepository componentRepo)
        => entities.Where(world.Events.WasAdded).ToHashSet();

    public static bool Has(Entity entity)
        => entity.World.Events.WasAdded(entity.Id);
}

