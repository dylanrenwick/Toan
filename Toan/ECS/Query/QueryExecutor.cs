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

    public IReadOnlySet<Guid> Execute()
    {
        ISet<Guid> queryResults = Entities;

        foreach (var queryType in Types)
        {
            if (queryType.ImplementsInterface(typeof(IComponent)))
            {
                queryResults = Reduce(queryResults, Components, queryType);
                continue;
            }

            var reduceMethod = queryType.GetMethod(
                "Reduce",
                BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Static
                | BindingFlags.FlattenHierarchy
            );

            if (reduceMethod != null)
            {
                var result = reduceMethod.Invoke(
                        null,
                        new object[]
                        {
                            queryResults,
                            Components
                        }
                    );

                if (result is ISet<Guid> resultSet)
                    queryResults = resultSet;
                else
                    throw new ArgumentException($"{queryType.FullName} is not a valid IWorldQueryable");
            }
            else
                throw new ArgumentException($"{queryType.FullName} could not be evaluated to a WorldQueryable, static method 'Reduce' not found");
        }

        return (IReadOnlySet<Guid>)queryResults;
    }

    private static ISet<Guid> Reduce(ISet<Guid> entities, ComponentRepository componentRepo, Type reducingType)
    {
        return entities
            .Where(entityId => componentRepo.Has(entityId, reducingType))
            .ToHashSet();
    }
}
