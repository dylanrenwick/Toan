using System;
using System.Collections.Generic;
using Toan.ECS.Query;

namespace Toan.ECS.Systems;

public interface IEntitySystem
{
    public IWorldQuery Archetype { get; }

	public void UpdateComponents(World world, IReadOnlySet<Guid> entities);
}
