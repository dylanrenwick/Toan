using System;
using System.Collections.Generic;

namespace Toan.ECS;

public interface IQuery
{
    public IReadOnlySet<Type> Types();

	public IReadOnlySet<Guid> GetEntities(World world);
}
