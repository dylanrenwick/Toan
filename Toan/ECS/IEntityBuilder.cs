using System;

using Toan.ECS.Bundles;

namespace Toan.ECS;

public interface IEntityBuilder<TSelf>
    where TSelf : IEntityBuilder<TSelf>
{
    public Guid Id { get; }

    public TSelf With<T>()
        where T : struct;
    public TSelf With<T>(T component)
        where T : struct;

    public TSelf WithIfNew<T>()
        where T : struct;
    public TSelf WithIfNew<T>(T component)
        where T : struct;

    public TSelf WithBundle(IBundle bundle);

    public TSelf Without<T>()
        where T : struct;

    public bool Has<T>()
        where T : struct;
}
