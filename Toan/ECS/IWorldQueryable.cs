using System;
using System.Collections.Generic;

namespace Toan.ECS;
public interface IWorldQueryable
{
    public ISet<Guid> Reduce(ISet<Guid> entities, World world);
}
