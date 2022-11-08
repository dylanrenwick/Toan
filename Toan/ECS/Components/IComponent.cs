using System;
using System.Collections.Generic;

using Toan.ECS.Query;

namespace Toan.ECS.Components;
public interface IComponent : IWorldQueryable
{
    ISet<Guid> IWorldQueryable.Reduce(World world, ISet<Guid> entities, ComponentRepository componentRepo)
        => entities;
}
