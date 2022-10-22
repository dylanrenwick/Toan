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

    private ComponentSet _components { get; init; }

    public Entity(Guid id, World world, ComponentSet components)
    {
        Id = id;
        World = world;

        _components = components;
    }

    /// <summary>
    /// Adds a new component of type <typeparamref name="TComponent"/> to the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public Entity With<TComponent>()
        where TComponent : GameComponent, new()
    => With(new TComponent());
    /// <summary>
    /// Adds a component to the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public Entity With(GameComponent component)
    {
        World.Dirty();
        _components.Add(component);
        return this;
    } 
    /// <summary>
    /// Adds a sequence of components to the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public Entity With(IEnumerable<GameComponent> components)
    {
        World.Dirty();
        foreach (var component in components)
        {
            _components.Add(component);
        }
        return this;
    }

    /// <summary>
    /// Removes a component of type <typeparamref name="TComponent"/> from the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public Entity Without<TComponent>()
        where TComponent : GameComponent
    {
        World.Dirty();
        _components.Remove<TComponent>();
        return this;
    }
    /// <summary>
    /// Removes a component from the entity by component <see cref="Guid"/>
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public Entity Without(Guid componentId)
    {
        World.Dirty();
        var found = _components.Where(component => component.Id == componentId);
        if (found.Any()) _components.Remove(found.First());
        return this;
    }

    public bool Has<TComponent>()
        where TComponent : GameComponent
    => _components.Has<TComponent>();

    public bool Has(Type type)
    => _components.Has(type);

    public TComponent Get<TComponent>()
        where TComponent : GameComponent
    => _components.Get<TComponent>();
}
