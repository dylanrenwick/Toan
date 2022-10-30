using System;
using System.Collections.Generic;
using System.Linq;
using Toan.ECS.Components;

namespace Toan.ECS.Query;

public class Without<T> : IWorldQueryable
    where T : struct, IWorldQueryable
{
    public static ISet<Guid> Reduce(ISet<Guid> entities, ComponentRepository componentRepo)
    {
        return entities
            .Where(entityId => !componentRepo.Has(entityId, typeof(T)))
            .ToHashSet();
    }
}
