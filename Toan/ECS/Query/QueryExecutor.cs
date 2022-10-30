using System;
using System.Collections.Generic;

using Toan.ECS.Components;

namespace Toan.ECS.Query;

public class QueryExecutor
{
    public required ComponentRepository Components { private get; init; }
    public required IReadOnlySet<Guid> Entities { private get; init; }
    public required IReadOnlySet<Type> Types { private get; init; }

    public IReadOnlySet<Guid> Execute()
    {
        IReadOnlySet<Guid> queryResults = Entities;

        foreach (var queryType in Types)
        {
            var result = queryType
                .GetMethod("Reduce")
                ?.Invoke(
                    null,
                    new object[]
                    {
                        queryResults,
                        Components
                    }
                );

            if (result is IReadOnlySet<Guid> resultSet)
                queryResults = resultSet;
            else
                throw new ArgumentException($"{queryType.FullName} is not a valid IWorldQueryable");
        }

        return queryResults;
    }
}
