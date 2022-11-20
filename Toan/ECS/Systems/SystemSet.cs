using System.Collections.Generic;
using System.Linq;

namespace Toan.ECS.Systems;

public class SystemSet<T> : SortedSet<SystemGroup<T>>
    where T : IGameSystem
{
    private int NextIndex => base.Reverse()
        .Cast<SystemGroup<T>?>()
        .FirstOrDefault((SystemGroup<T>?)null)
        ?.Order + 1 ?? 0;

    public SystemSet()
        : base(new SystemOrderComparer())
    { }

    public int Add(T system)
    => Add(new SystemGroup<T>()
    {
        Order = NextIndex,
        Systems = new HashSet<T>() { system }
    });
    public int Add<TSystem>(IEnumerable<TSystem> systems)
        where TSystem : T
    => Add(new SystemGroup<T>()
    {
        Order = NextIndex,
        Systems = new HashSet<T>(systems.Cast<T>()),
    });
    public new int Add(SystemGroup<T> systemGroup)
    {
        base.Add(systemGroup);
        return systemGroup.Order;
    }

    private class SystemOrderComparer : Comparer<SystemGroup<T>>
    {
        public override int Compare(SystemGroup<T> x, SystemGroup<T> y)
            => x.Order - y.Order;
    }
}

