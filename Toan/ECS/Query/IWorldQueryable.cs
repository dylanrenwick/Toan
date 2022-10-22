using System;
using System.Collections.Generic;

namespace Toan.ECS.Query;

public interface IWorldQueryable
{
    public ISet<Guid> Reduce(ISet<Guid> entities, World world);
}
