using System;

using Toan.ECS.Bundles;
using Toan.ECS.Components;

namespace Toan.ECS;

/// <summary>
/// A temporary, lightweight wrapper around a set of <see cref="GameComponent"/>s, a <see cref="World"/> reference, and a <see cref="Guid"/>
/// </summary>
public class Entity : BaseEntity
{
    public override bool IsReal { get => true; }

    public required ComponentRepository Components { private get; init; }

    public override IEntity With<T>(T component)
        where T : struct
    {
        Dirty<T>();
        Components.Add(Id, component);
        return this;
    } 

    public override IEntity Without<T>()
        where T : struct
    {
        if (Components.Remove<T>(Id))
            Dirty();
        return this;
    }

    public override bool Has<TComponent>()
        where TComponent : struct
    => Components.Has<TComponent>(Id);

    public override int Count()
        => Components.Count(Id);

    public override TComponent Get<TComponent>()
        where TComponent : struct
    => Components.Get<TComponent>(Id);
    public override object[] GetAll()
        => Components.GetAll(Id);

    public override Guid Make()
        => Id;

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

