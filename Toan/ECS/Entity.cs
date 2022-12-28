using System;

using Toan.ECS.Bundles;
using Toan.ECS.Components;

namespace Toan.ECS;

/// <summary>
/// A temporary, lightweight wrapper around a set of <see cref="GameComponent"/>s, a <see cref="World"/> reference, and a <see cref="Guid"/>
/// </summary>
public readonly struct Entity
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
    => With(new T());
    /// <summary>
    /// Adds a component to the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public Entity With<T>(T component)
        where T : struct
    {
        Dirty<T>();
        Components.Add(Id, component);
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
        if (Components.Remove<T>(Id))
            Dirty();
        return this;
    }

    public bool Has<TComponent>()
        where TComponent : struct
    => Components.Has<TComponent>(Id);

    public bool Has(Type type)
    => Components.Has(Id, type);

    public TComponent Get<TComponent>()
        where TComponent : struct
    => Components.Get<TComponent>(Id);

    private void Dirty()
    {
        World.Dirty(Id);
    }

    private void Dirty<T>()
        where T : struct
    {
        World.Dirty<T>(Id);
    }
}

