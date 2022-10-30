using System;
using System.Collections.Generic;
using System.Linq;
using Toan.ECS.Components;

namespace Toan.ECS;

/// <summary>
/// A temporary, lightweight wrapper around a set of <see cref="GameComponent"/>s, a <see cref="World"/> reference, and a <see cref="Guid"/>
/// </summary>
public struct Entity
{
    public Guid Id { get; init; }
    public World World { get; init; }

    private ComponentRepository _components { get; init; }

    public Entity(Guid id, World world, ComponentRepository components)
    {
        Id = id;
        World = world;

        _components = components;
    }

    /// <summary>
    /// Adds a new component of type <typeparamref name="T"/> to the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public Entity With<T>()
        where T : struct
    => With<T>(new T());
    /// <summary>
    /// Adds a component to the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public Entity With<T>(T component)
        where T : struct
    {
        World.Dirty();
        _components.Add<T>(Id, component);
        return this;
    } 

    /// <summary>
    /// Removes a component of type <typeparamref name="T"/> from the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public Entity Without<T>()
        where T : struct
    {
        World.Dirty();
        _components.Remove<T>(Id);
        return this;
    }

    public bool Has<TComponent>()
        where TComponent : struct
    => _components.Has<TComponent>(Id);

    public bool Has(Type type)
    => _components.Has(Id, type);

    public TComponent Get<TComponent>()
        where TComponent : struct
    => _components.Get<TComponent>(Id)
        ?? throw new ArgumentException($"Entity {Id} does not have a component of type {typeof(TComponent).FullName}");
}
