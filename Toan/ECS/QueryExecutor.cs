using System;
using System.Collections.Generic;
using System.Linq;
using Toan.ECS.Components;

namespace Toan.ECS;

public class QueryExecutor
{
	public required IReadOnlyDictionary<Guid, ComponentSet> Entities { get; init; }
	public required IReadOnlySet<Type> Types { get; init; }

	public IReadOnlySet<Guid> Execute()
	{
		HashSet<Guid> results = new();

		foreach ((var entityId, var components) in Entities.Tuplize())
		{
			if (Types.All(components.Has))
				results.Add(entityId);
		}

		return results;
	}
}
