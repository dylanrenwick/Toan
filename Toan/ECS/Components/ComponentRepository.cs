using System;
using System.Collections.Generic;

namespace Toan.ECS.Components;

public class ComponentRepository
{
    private readonly Dictionary<Type, IComponentPool> _componentPools = new();

    private void AddPool<T>()
        where T : struct
    {
        _componentPools.Add(typeof(T), new ComponentPool<T>());
    }

    public bool HasPool<T>()
        where T : struct
    => HasPool(typeof(T));
    public bool HasPool(Type t)
    {
        return _componentPools.ContainsKey(t);
    }

    public void Add<T>(Guid entityId, T component)
        where T : struct
    {
        if (!HasPool<T>())
            AddPool<T>();

        _componentPools[typeof(T)].Add(entityId, component);
    }

    public bool RemoveAll(Guid entityId)
    {
        bool success = false;
        foreach (Type type in _componentPools.Keys)
        {
            success |= Remove(entityId, type);
        }
        return success;
    }

    public bool Remove<T>(Guid entityId)
        where T : struct
    => Remove(entityId, typeof(T));
    public bool Remove(Guid entityId, Type t)
    {
        if (!HasPool(t))
            return false;

        return _componentPools[t].Remove(entityId);
    }

    public bool Has<T>(Guid entityId)
        where T : struct
    => Has(entityId, typeof(T));
    public bool Has(Guid entityId, Type t)
    {
        if (!HasPool(t))
            return false;

        return _componentPools[t].HasEntity(entityId);
    }

    public T Get<T>(Guid entityId)
        where T : struct
    {
        if (!HasPool<T>())
            throw new ArgumentException($"No component pool exists for component type {typeof(T).FullName}");

        var pool = (ComponentPool<T>)_componentPools[typeof(T)];
        return pool.Get(entityId);
    }
}
