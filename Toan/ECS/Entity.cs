using System;
using System.Collections.Generic;
using System.Linq;
using Toan.ECS.Bundles;
using Toan.ECS.Components;

namespace Toan.ECS;

/// <summary>
/// A temporary, lightweight wrapper around a set of <see cref="GameComponent"/>s, a <see cref="World"/> reference, and a <see cref="Guid"/>
/// </summary>
public struct Entity
{
    public required Guid Id { get; init; }
    public required World World { get; init; }

    public required ComponentRepository Components { private get; init; }

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
        Dirty();
        Components.Add<T>(Id, component);
        return this;
    } 

    public Entity WithBundle(IBundle bundle)
    {
        Dirty();
        bundle.AddBundle(Id, Components);
        return this;
    }

    /// <summary>
    /// Removes a component of type <typeparamref name="T"/> from the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public Entity Without<T>()
        where T : struct
    {
        Dirty();
        Components.Remove<T>(Id);
        return this;
    }

    public bool Has<TComponent>()
        where TComponent : struct
    => Components.Has<TComponent>(Id);

    public bool Has(Type type)
    => Components.Has(Id, type);

    public ref TComponent Get<TComponent>()
        where TComponent : struct
    => ref Components.Get<TComponent>(Id);

    private void Dirty()
    {
        World.Dirty(Id);
    }
}

