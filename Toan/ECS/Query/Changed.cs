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
    public virtual ISet<Guid> Reduce(World world, ISet<Guid> entities, ComponentRepository componentRepo)
        => entities.Where(world.Events.WasChanged).ToHashSet();

    public static bool Has(Entity entity)
        => entity.World.Events.WasChanged(entity.Id);
}

public class Changed<T> : Changed
    where T : struct
{
    public override ISet<Guid> Reduce(World world, ISet<Guid> entities, ComponentRepository componentRepo)
        => entities.Where(world.Events.WasChanged<T>).ToHashSet();
}
