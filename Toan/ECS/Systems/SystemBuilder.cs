using System;
using System.Collections.Generic;
using System.Linq;

namespace Toan.ECS.Systems;

public struct SystemBuilder
{
    public required World World { get; init; }

    public required SystemSet<IUpdateSystem> UpdateSystems { private get; init; }
    public required SystemSet<IRenderSystem> RenderSystems { private get; init; }
    public required HashSet<IEntitySystem> EntitySystems { private get; init; }

    private int? _nextUpdateIndex;
    private int? _nextRenderIndex;

    public SystemBuilder Add<TSystem>()
        where TSystem : IGameSystem, new()
        => Add(new TSystem());
    public SystemBuilder Add(IGameSystem system)
    {
        AddByType(system);

        return this;
    }

    public SystemBuilder Add<TSystem>(params TSystem[] systems)
        where TSystem : IGameSystem
    {
        AddByType(systems.ToHashSet());

        return this;
    }

    private void AddByType<TSystem>(TSystem system)
        where TSystem : IGameSystem
        => AddByType(new HashSet<TSystem>() { system });
    private void AddByType<TSystem>(IReadOnlySet<TSystem> systems)
        where TSystem : IGameSystem
    {
        ByType(
            systems: systems,
            update: UpdateSystems.Add,
            render: RenderSystems.Add,
            entity: EntitySystems.UnionWith
        );
    }

    private void ByType<TSystem>(
        IReadOnlySet<TSystem> systems,
        Func<SystemGroup<IUpdateSystem>, int> update,
        Func<SystemGroup<IRenderSystem>, int> render,
        Action<IReadOnlySet<IEntitySystem>> entity
    ) where TSystem : IGameSystem
    {
        Type systemType = typeof(TSystem);
        var (isUpdateable, isRenderable, isEntitySys) = GetSystemTypes(systemType);

        if (!isUpdateable && !isRenderable && !isEntitySys)
			throw new ArgumentException($"System type of {systemType.Name} is not renderable, updatable, or an entity system");

        if (isUpdateable)
            _nextUpdateIndex = update(new()
            {
                Order = (_nextUpdateIndex ?? GetLastUpdateOrder()) + 1,
                Systems = systems.Cast<IUpdateSystem>().ToHashSet(),
            });
        if (isRenderable)
            _nextRenderIndex = render(new()
            {
                Order = (_nextRenderIndex ?? GetLastRenderOrder()) + 1,
                Systems= systems.Cast<IRenderSystem>().ToHashSet(),
            });
        if (isEntitySys)
            entity(systems.Cast<IEntitySystem>().ToHashSet());
    }

    private int GetLastUpdateOrder()
        => GetLastOrder(UpdateSystems);
    private int GetLastRenderOrder()
        => GetLastOrder(RenderSystems);

    private static int GetLastOrder<TSystem>(SystemSet<TSystem> systems)
        where TSystem : IGameSystem
    => systems.Count > 0
        ? systems.Reverse().First().Order
        : 0;

    private static (bool, bool, bool) GetSystemTypes(Type type)
    => (
        type == typeof(IUpdateSystem) || type.ImplementsInterface(typeof(IUpdateSystem)),
        type == typeof(IRenderSystem) || type.ImplementsInterface(typeof(IRenderSystem)),
        type == typeof(IEntitySystem) || type.ImplementsInterface(typeof(IEntitySystem))
    );
}

