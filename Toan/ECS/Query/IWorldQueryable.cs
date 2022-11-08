using System;
using System.Collections.Generic;

using Toan.ECS.Components;

namespace Toan.ECS.Query;

public interface IWorldQueryable
{
    public ISet<Guid> Reduce(World world, ISet<Guid> entities, ComponentRepository componentRepo);
}
