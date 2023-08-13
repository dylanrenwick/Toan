using System;
using System.Collections.Generic;
using System.Linq;

namespace Toan.ECS.Query;

public class WorldQuery : IWorldQuery
{
    protected HashSet<Type> _types = new();

    public virtual IReadOnlySet<Type> Types() => _types;

    public IReadOnlySet<Guid> GetEntities(World world)
    {
        var executor = world.GetQueryExecutor(Types());

        return executor.Execute();
    }

    public static WorldQuery FromTypes(IReadOnlySet<Type> types)
    {
        WorldQuery query = new() { _types = types.ToHashSet() };
        return query;
    }
}

public class WorldQuery<TQueryable> : WorldQuery
    where TQueryable : IWorldQueryable
{
    public override IReadOnlySet<Type> Types()
    {
        if (_types.Count != 1)
        {
            _types.Clear();
            _types.Add(typeof(TQueryable));
        }

        return base.Types();
    }
}

public class WorldQuery<TQueryable1, TQueryable2> : WorldQuery
    where TQueryable1 : IWorldQueryable 
    where TQueryable2 : IWorldQueryable
{
    public override IReadOnlySet<Type> Types()
    {
        if (_types.Count != 1)
        {
            _types.Clear();
            _types.Add(typeof(TQueryable1));
            _types.Add(typeof(TQueryable2));
        }

        return base.Types();
    }
}

public class WorldQuery<TQueryable1, TQueryable2, TQueryable3> : WorldQuery
    where TQueryable1 : IWorldQueryable
    where TQueryable2 : IWorldQueryable
    where TQueryable3 : IWorldQueryable
{
    public override IReadOnlySet<Type> Types()
    {
        if (_types.Count != 1)
        {
            _types.Clear();
            _types.Add(typeof(TQueryable1));
            _types.Add(typeof(TQueryable2));
            _types.Add(typeof(TQueryable3));
        }

        return base.Types();
    }
}

public class WorldQuery<TQueryable1, TQueryable2, TQueryable3, TQueryable4> : WorldQuery
    where TQueryable1 : IWorldQueryable
    where TQueryable2 : IWorldQueryable
    where TQueryable3 : IWorldQueryable
    where TQueryable4 : IWorldQueryable
{
    public override IReadOnlySet<Type> Types()
    {
        if (_types.Count != 1)
        {
            _types.Clear();
            _types.Add(typeof(TQueryable1));
            _types.Add(typeof(TQueryable2));
            _types.Add(typeof(TQueryable3));
            _types.Add(typeof(TQueryable4));
        }

        return base.Types();
    }
}

public class WorldQuery<TQueryable1, TQueryable2, TQueryable3, TQueryable4, TQueryable5> : WorldQuery
    where TQueryable1 : IWorldQueryable
    where TQueryable2 : IWorldQueryable
    where TQueryable3 : IWorldQueryable
    where TQueryable4 : IWorldQueryable
    where TQueryable5 : IWorldQueryable
{
    public override IReadOnlySet<Type> Types()
    {
        if (_types.Count != 1)
        {
            _types.Clear();
            _types.Add(typeof(TQueryable1));
            _types.Add(typeof(TQueryable2));
            _types.Add(typeof(TQueryable3));
            _types.Add(typeof(TQueryable4));
            _types.Add(typeof(TQueryable5));
        }

        return base.Types();
    }
}
