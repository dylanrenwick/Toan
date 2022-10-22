﻿using System;
using System.Collections.Generic;

namespace Toan.ECS.Systems;

public interface IEntitySystem
{
    public IWorldQuery Archetype { get; }

	public void UpdateComponents(World world, IReadOnlySet<Guid> entities);
}
