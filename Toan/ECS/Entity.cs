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
    public required World World { get; init; }
    public required Guid Id { get; init; }
    public required ComponentSet Components { get; init; }

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
        Components.Add(component);
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
            Components.Add(component);
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
        Components.Remove<TComponent>();
        return this;
    }
    /// <summary>
    /// Removes a component from the entity by component <see cref="Guid"/>
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public Entity Without(Guid componentId)
    {
        World.Dirty();
        var found = Components.Where(component => component.Id == componentId);
        if (found.Any()) Components.Remove(found.First());
        return this;
    }

    public bool Has<TComponent>()
        where TComponent : GameComponent
    => Components.Has<TComponent>();

    public bool Has(Type type)
    => Components.Has(type);

    public TComponent Get<TComponent>()
        where TComponent : GameComponent
    => Components.Get<TComponent>();
}
