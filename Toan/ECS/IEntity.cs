using System;

using Toan.ECS.Bundles;

namespace Toan.ECS;

public interface IEntity
{
    public Guid Id { get; }

    public World World { get; }

    public bool IsReal { get; }

    /// <summary>
    /// Adds a new component of type <typeparamref name="T"/> to the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public IEntity With<T>()
        where T : struct;
    /// <summary>
    /// Adds a component to the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public IEntity With<T>(T component)
        where T : struct;

    public IEntity WithIfNew<T>()
        where T : struct;
    public IEntity WithIfNew<T>(T component)
        where T : struct;

    public IEntity WithBundle(IBundle bundle);

    /// <summary>
    /// Removes a component of type <typeparamref name="T"/> from the entity
    /// </summary>
    /// <returns>This <see cref="Entity"/> for chaining purposes</returns>
    public IEntity Without<T>()
        where T : struct;

    public bool Has<T>()
        where T : struct;

    public int Count();

    public T Get<T>()
        where T : struct;

    public ValueType[] GetAll();
    public Guid Make();
}
