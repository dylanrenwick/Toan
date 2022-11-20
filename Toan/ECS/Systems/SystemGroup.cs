using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Toan.Rendering;

namespace Toan.ECS.Systems;

public struct SystemGroup<T> : IReadOnlySet<T>
    where T : IGameSystem
{
    public required int Order { get; init; }
    public required IReadOnlySet<T> Systems { get; init; }

    public void Update(World world, GameTime gameTime)
    {
        if (typeof(T).ImplementsInterface(typeof(IUpdateSystem)))

        foreach (var system in Systems.Cast<IUpdateSystem>())
        {
            system.Update(world, gameTime);
        }
    }

    public void Render(World world, Renderer renderer, GameTime gameTime)
    {
        if (typeof(T).ImplementsInterface(typeof(IRenderSystem)))

        foreach (var system in Systems.Cast<IRenderSystem>())
        {
            system.Render(world, renderer, gameTime);
        }
    }

    public SystemGroup<TSystem> CastSystem<TSystem>()
        where TSystem : IGameSystem
    {
        return new()
        {
            Order = Order,
            Systems = Systems.Cast<TSystem>().ToHashSet(),
        };
    }

    #region IReadOnlySet<T> Implementation
    public int Count => Systems.Count;

    public bool Contains(T item) => Systems.Contains(item);
    public IEnumerator<T> GetEnumerator() => Systems.GetEnumerator();
    public bool IsProperSubsetOf(IEnumerable<T> other) => Systems.IsProperSubsetOf(other);
    public bool IsProperSupersetOf(IEnumerable<T> other) => Systems.IsProperSupersetOf(other);
    public bool IsSubsetOf(IEnumerable<T> other) => Systems.IsSubsetOf(other);
    public bool IsSupersetOf(IEnumerable<T> other) => Systems.IsSupersetOf(other);
    public bool Overlaps(IEnumerable<T> other) => Systems.Overlaps(other);
    public bool SetEquals(IEnumerable<T> other) => Systems.SetEquals(other);
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Systems).GetEnumerator();
    #endregion
}

