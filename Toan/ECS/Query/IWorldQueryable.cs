using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Toan.ECS.Components;

namespace Toan.ECS.Query;

public interface IWorldQueryable
{
    public static virtual ISet<Guid> Reduce(ISet<Guid> entities, ComponentRepository componentRepo)
    {
        Type? type = MethodBase.GetCurrentMethod()?.DeclaringType;
        if (type == null)
            return entities;

        return entities
            .Where(entityId => componentRepo.Has(entityId, type))
            .ToHashSet();
    }
}
