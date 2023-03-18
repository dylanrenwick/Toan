using System;
using System.Collections.Generic;

namespace Toan.ECS.Components;

/// <summary>
/// Maintains <see cref="ComponentPool{TComponent}"/>s for each unique component type.
/// 
/// Acts as a collective container of all components in the ECS.
/// </summary>
public class ComponentRepository
{
    private readonly Dictionary<Type, IComponentPool> _componentPools = new();

    private void AddPool<T>()
        where T : struct
    {
        _componentPools.Add(typeof(T), new ComponentPool<T>());
    }
    private void AddPool(Type type)
    {
        Type poolType = typeof(ComponentPool<>).MakeGenericType(type);
        var newPool = (IComponentPool)(Activator.CreateInstance(poolType)
            ?? throw new Exception($"Failed to create pool of type {poolType}"));
        _componentPools.Add(type, newPool);
    }

    /// <summary>
    /// Checks whether the repository already contains a pool for the given component type.
    /// </summary>
    /// <param name="t">The component type to look for a pool of</param>
    /// <returns>true if a pool for the given component was found, false otherwise</returns>
    public bool HasPool(Type t)
    {
        return _componentPools.ContainsKey(t);
    }
    /// <summary>
    /// Checks whether the repository already contains a pool for the given component type.
    /// </summary>
    /// <typeparam name="T">The component type to look for a pool of</typeparam>
    /// <returns>true if a pool for the given component was found, false otherwise</returns>
    public bool HasPool<T>()
        where T : struct
    => HasPool(typeof(T));

    /// <summary>
    /// Adds a component to the repository.
    /// 
    /// Creates a new pool for the component type if one does not already exist.
    /// </summary>
    /// <typeparam name="T">The component type being added</typeparam>
    /// <param name="entityId">Guid ID of the entity the component is being added to</param>
    /// <param name="component">The component being added</param>
    public void Add<T>(Guid entityId, T component)
        where T : struct
    {
        if (!HasPool<T>())
            AddPool<T>();

        _componentPools[typeof(T)].Add(entityId, component);
    }

    public void Add(Guid entityId, Type t, object component)
    {
        if (!HasPool(t))
            AddPool(t);

        _componentPools[t].Add(entityId, component);
    }

    public void AddAll(Guid entityId, object[] components)
    {
        foreach (var component in components)
        {
            Add(entityId, component.GetType(), component);
        }
    }

    /// <summary>
    /// Removes all components for a given entity from the repository.
    /// </summary>
    /// <param name="entityId">The ID of the entity to remove components of</param>
    /// <returns>false if no components were removed, true otherwise</returns>
    public bool RemoveAll(Guid entityId)
    {
        bool success = false;
        foreach (Type type in _componentPools.Keys)
        {
            success |= Remove(entityId, type);
        }
        return success;
    }

    /// <summary>
    /// Removes a component of the given type from the given entity.
    /// </summary>
    /// <param name="entityId">The ID of the entity to remove the component from</param>
    /// <param name="t">The type of component to remove</param>
    /// <returns>false if no components were removed, true otherwise</returns>
    public bool Remove(Guid entityId, Type t)
    {
        if (!HasPool(t))
            return false;

        return _componentPools[t].Remove(entityId);
    }
    /// <summary>
    /// Removes a component of the given type from the given entity.
    /// </summary>
    /// <typeparam name="T">The type of component to remove</typeparam>
    /// <param name="entityId">The ID of the entity to remove the component from</param>
    /// <returns>false if no components were removed, true otherwise</returns>
    public bool Remove<T>(Guid entityId)
        where T : struct
    => Remove(entityId, typeof(T));

    /// <summary>
    /// Checks whether the given entity has a component of the given type.
    /// </summary>
    /// <param name="entityId">The ID of the entity to look for a component on</param>
    /// <param name="t">The type of component to look for</param>
    /// <returns>true if a component was found, false otherwise</returns>
    public bool Has(Guid entityId, Type t)
    {
        if (!HasPool(t))
            return false;

        return _componentPools[t].HasEntity(entityId);
    }
    /// <summary>
    /// Checks whether the given entity has a component of the given type.
    /// </summary>
    /// <typeparam name="T">The type of component to look for</typeparam>
    /// <param name="entityId">The ID of the entity to look for a component on</param>
    /// <returns>true if a component was found, false otherwise</returns>
    public bool Has<T>(Guid entityId)
        where T : struct
    => Has(entityId, typeof(T));

    /// <summary>
    /// Retrieves a component from the repository by component type and entity ID.
    /// </summary>
    /// <typeparam name="T">The type of component to look for</typeparam>
    /// <param name="entityId">The ID of the entity to look for a component on</param>
    /// <returns>The found component</returns>
    /// <exception cref="ArgumentException">No component of type was found on entity</exception>
    public T Get<T>(Guid entityId)
        where T : struct
    {
        if (!HasPool<T>())
            throw new ArgumentException($"No component pool exists for component type {typeof(T).FullName}");

        var pool = (ComponentPool<T>)_componentPools[typeof(T)];
        return pool.Get(entityId);
    }

    public object Get(Guid entityId, Type t)
    {
        if (!HasPool(t))
            throw new ArgumentException($"No component pool exists for component type {t.FullName}");

        Type poolType = typeof(ComponentPool<>).MakeGenericType(t);
        var getMethod = poolType.GetMethod("Get")
            ?? throw new Exception($"Failed to get method 'Get' on type {poolType}");
        return getMethod.Invoke(_componentPools[t], new object[] { entityId })
            ?? throw new Exception($"Failed to call {getMethod.Name} on pool of type {poolType.FullName}");
    }

    public object[] GetAll(Guid entityId)
    {
        var components = new List<object>();
        foreach (Type type in _componentPools.Keys)
        {
            if (Has(entityId, type))
                components.Add(Get(entityId, type));
        }
        return components.ToArray();
    }
}
