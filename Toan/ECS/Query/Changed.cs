using System;
using System.Collections.Generic;
using System.Linq;

using Toan.ECS.Components;

namespace Toan.ECS.Query;

/// <summary>
/// Indicates that the query should only match entities whos components have changed since the last update.
/// </summary>
public class Changed : IWorldQueryable
{
    public ISet<Guid> Reduce(World world, ISet<Guid> entities, ComponentRepository componentRepo)
        => entities.Where(world.Events.WasChanged).ToHashSet();
}
