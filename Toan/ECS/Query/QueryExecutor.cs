using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Toan.ECS.Components;

namespace Toan.ECS.Query;

public class QueryExecutor
{
    public required ComponentRepository Components { private get; init; }
    public required ISet<Guid> Entities { private get; init; }
    public required IReadOnlySet<Type> Types { private get; init; }
    public required World World { private get; init; }

    public IReadOnlySet<Guid> Execute()
    {
        ISet<Guid> queryResults = Entities;

        foreach (var queryType in Types)
        {
            if (queryType.ImplementsInterface(typeof(IComponent)))
                queryResults = ComponentReduce(queryResults, Components, queryType);
            else if (Activator.CreateInstance(queryType) is IWorldQueryable query)
                queryResults = query.Reduce(World, queryResults, Components);
            else
                throw new ArgumentException($"{queryType.FullName} is not a valid IWorldQueryable");
        }

        return (IReadOnlySet<Guid>)queryResults;
    }

    private static ISet<Guid> ComponentReduce(ISet<Guid> entities, ComponentRepository componentRepo, Type reducingType)
    {
        return entities
            .Where(entityId => componentRepo.Has(entityId, reducingType))
            .ToHashSet();
    }
}
