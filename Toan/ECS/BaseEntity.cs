using System;

using Toan.ECS.Bundles;

namespace Toan.ECS;

public abstract class BaseEntity : IEntity
{
    public required Guid Id { get; init; }
    public required World World { get; init; }

    public virtual bool IsReal { get => false; }

    public abstract T Get<T>()
        where T : struct;
    public abstract object[] GetAll();
    public abstract bool Has<T>()
        where T : struct;
    public abstract int Count();
    public IEntity With<T>()
        where T : struct
    => With<T>(new());
    public abstract IEntity With<T>(T component)
        where T : struct;
    public abstract IEntity Without<T>()
        where T : struct;

    public IEntity WithBundle(IBundle bundle)
    {
        bundle.AddBundle(this);
        return this;
    }

    public IEntity WithIfNew<T>()
        where T : struct
    {
        if (!Has<T>())
            With<T>();
        return this;
    }
    public IEntity WithIfNew<T>(T component)
        where T : struct
    {
        if (!Has<T>())
            With(component);

        return this;
    }

    public virtual Guid Make()
    {
        World.MakeEntity(this);
        return Id;
    }
}
