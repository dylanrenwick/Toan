using System;

namespace Toan.ECS;

public interface IEntityBuilder<TSelf>
    where TSelf : struct, IEntityBuilder<TSelf>
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

    public TSelf Without<T>()
        where T : struct;

    public bool Has<T>()
        where T : struct;
}
