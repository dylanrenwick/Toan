﻿using System;
using System.Collections.Generic;
using System.Linq;

using Toan.ECS.Components;

namespace Toan.ECS;

public class Query : IQuery
{
    protected HashSet<Type> _types = new();

    public virtual IReadOnlySet<Type> Types()
    {
        return _types;
    }

	public IReadOnlySet<Guid> GetEntities(World world)
	{
		var entities = world.Entities;
		QueryExecutor executor = new QueryExecutor
		{
			Entities = entities,
			Types = Types(),
		};

		return executor.Execute();
	}

    public static Query FromTypes(IReadOnlySet<Type> types)
    {
        Query query = new() { _types = types.ToHashSet() };
        return query;
    }
}

public class Query<TComponent> : Query
    where TComponent : GameComponent
{
    public override IReadOnlySet<Type> Types()
    {
		if (_types.Count != 1)
		{
			_types.Clear();
			_types.Add(typeof(TComponent));
		}

		return base.Types(); 
    }

    public IEnumerable<(Guid, TComponent)> Enumerate(World world)
        => Enumerate(world, this.GetEntities(world));

    public IEnumerable<(Guid, TComponent)> Enumerate(World world, IReadOnlySet<Guid> entityIds)
    {
        return entityIds
            .Select(entityId =>
            {
                var components = world.Components(entityId);
                return (
                    entityId,
                    components.Get<TComponent>()
               );
            });
    }
}

public class Query<TComponent1, TComponent2> : Query
    where TComponent1 : GameComponent
    where TComponent2 : GameComponent
{
    public override IReadOnlySet<Type> Types()
    {
		if (_types.Count != 1)
		{
			_types.Clear();
			_types.Add(typeof(TComponent1));
			_types.Add(typeof(TComponent2));
		}

		return base.Types(); 
    }

    public IEnumerable<(Guid, TComponent1, TComponent2)> Enumerate(World world)
        => Enumerate(world, this.GetEntities(world));

    public IEnumerable<(Guid, TComponent1, TComponent2)> Enumerate(World world, IReadOnlySet<Guid> entityIds)
    {
        return entityIds
            .Select(entityId =>
            {
                var components = world.Components(entityId);
                return (
                    entityId,
                    components.Get<TComponent1>(),
                    components.Get<TComponent2>()
                );
            });
    }
}

public class Query<TComponent1, TComponent2, TComponent3> : Query
    where TComponent1 : GameComponent
    where TComponent2 : GameComponent
    where TComponent3 : GameComponent
{
    public override IReadOnlySet<Type> Types()
    {
		if (_types.Count != 1)
		{
			_types.Clear();
			_types.Add(typeof(TComponent1));
			_types.Add(typeof(TComponent2));
			_types.Add(typeof(TComponent3));
		}

		return base.Types(); 
    }

    public IEnumerable<
        (Guid, TComponent1, TComponent2, TComponent3)
    > Enumerate(World world) => Enumerate(world, this.GetEntities(world));

    public IEnumerable<
        (Guid, TComponent1, TComponent2, TComponent3)
    > Enumerate(World world, IReadOnlySet<Guid> entityIds)
    {
        return entityIds 
            .Select(entityId =>
            {
                var components = world.Components(entityId);
                return (
                    entityId,
                    components.Get<TComponent1>(),
                    components.Get<TComponent2>(),
                    components.Get<TComponent3>()
                );
            });
    }
}

public class Query<TComponent1, TComponent2, TComponent3, TComponent4> : Query
    where TComponent1 : GameComponent
    where TComponent2 : GameComponent
    where TComponent3 : GameComponent
    where TComponent4 : GameComponent
{
    public override IReadOnlySet<Type> Types()
    {
		if (_types.Count != 1)
		{
			_types.Clear();
			_types.Add(typeof(TComponent1));
			_types.Add(typeof(TComponent2));
			_types.Add(typeof(TComponent3));
			_types.Add(typeof(TComponent4));
		}

		return base.Types(); 
    }

    public IEnumerable<
        (Guid, TComponent1, TComponent2, TComponent3, TComponent4)
    > Enumerate(World world) => Enumerate(world, this.GetEntities(world));

    public IEnumerable<
        (Guid, TComponent1, TComponent2, TComponent3, TComponent4)
    > Enumerate(World world, IReadOnlySet<Guid> entityIds)
    {
        return entityIds 
            .Select(entityId =>
            {
                var components = world.Components(entityId);
                return (
                    entityId, 
                    components.Get<TComponent1>(),
                    components.Get<TComponent2>(),
                    components.Get<TComponent3>(),
                    components.Get<TComponent4>()
                );
            });
    }
}

