using System.Collections;
using System.Collections.Generic;

using Toan.ECS.Components;

namespace Toan.Physics;

public class Collisions : GameComponent, IList<Collision>
{
    private readonly List<Collision> _collisions = new();

    #region IList implementation
    public Collision this[int index] { get => ((IList<Collision>)_collisions)[index]; set => ((IList<Collision>)_collisions)[index] = value; }

    public int Count => ((ICollection<Collision>)_collisions).Count;

    public bool IsReadOnly => ((ICollection<Collision>)_collisions).IsReadOnly;

    public void Add(Collision item)
    {
        ((ICollection<Collision>)_collisions).Add(item);
    }

    public void Clear()
    {
        ((ICollection<Collision>)_collisions).Clear();
    }

    public bool Contains(Collision item)
    {
        return ((ICollection<Collision>)_collisions).Contains(item);
    }

    public void CopyTo(Collision[] array, int arrayIndex)
    {
        ((ICollection<Collision>)_collisions).CopyTo(array, arrayIndex);
    }

    public IEnumerator<Collision> GetEnumerator()
    {
        return ((IEnumerable<Collision>)_collisions).GetEnumerator();
    }

    public int IndexOf(Collision item)
    {
        return ((IList<Collision>)_collisions).IndexOf(item);
    }

    public void Insert(int index, Collision item)
    {
        ((IList<Collision>)_collisions).Insert(index, item);
    }

    public bool Remove(Collision item)
    {
        return ((ICollection<Collision>)_collisions).Remove(item);
    }

    public void RemoveAt(int index)
    {
        ((IList<Collision>)_collisions).RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_collisions).GetEnumerator();
    }
    #endregion
}
