using System;
using System.Collections.Generic;

namespace Toan.ECS.Systems;

public interface IEntitySystem
{
    public IQuery Archetype { get; }

	public void UpdateComponents(World world, IReadOnlySet<Guid> entities);
}
