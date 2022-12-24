using System;
using System.Collections.Generic;
using System.Linq;

using Toan.ECS.Components;

namespace Toan.ECS.Query;

/// <summary>
/// Indicates that the query should only match entities whos components have changed since the last update.
/// </summary>
public class Moved : IWorldQueryable
{
    public ISet<Guid> Reduce(World world, ISet<Guid> entities, ComponentRepository componentRepo)
        => entities.Where(world.Events.WasMoved).ToHashSet();

    public static bool Has(Entity entity)
        => entity.World.Events.WasMoved(entity.Id);
}
