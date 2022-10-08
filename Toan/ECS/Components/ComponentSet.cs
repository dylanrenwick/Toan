using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Toan.ECS.Components;

public class ComponentSet : ISet<GameComponent>
{
    private readonly HashSet<GameComponent> _innerSet = new();

    public ComponentSet() { }
    public ComponentSet(HashSet<GameComponent> innerSet)
    {
        _innerSet = innerSet;
    }

    public bool Has<T>()
        where T : GameComponent
    {
        var found = _innerSet.WhereType<GameComponent, T>();
        return found.Any();
    }
	public bool Has(Type type)
	{
		var found = _innerSet.WhereType(type);
		return found.Any();
	}

    public T Get<T>()
        where T : GameComponent
    {
        var found = _innerSet.WhereType<GameComponent, T>();
        if (found.Any()) return found.First();
        else throw new Exception("Could not find component");
    }
    public T Get<T>(Guid id)
        where T : GameComponent
    {
        var found = _innerSet.Single(component => component.Id == id);
        if (found is T foundT) return foundT;
        else throw new Exception("Could not find component");

    }

    public bool Remove<T>()
        where T : GameComponent
    {
        var found = _innerSet.WhereType<GameComponent, T>();
        if (found.Any())
        {
            _innerSet.Remove(found.First());
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool Remove(Guid id)
    {
        var found = _innerSet.SingleOrDefault(component => component?.Id == id, null);
        if (found != null)
        {
            _innerSet.Remove(found);
            return true;
        }
        else
        {
            return false;
        }
    }

    #region ISet<GameComponent> implementation

    public int Count => ((ICollection<GameComponent>)_innerSet).Count;

    public bool IsReadOnly => ((ICollection<GameComponent>)_innerSet).IsReadOnly;

    public bool Add(GameComponent item)
    {
        return ((ISet<GameComponent>)_innerSet).Add(item);
    }

    public void Clear()
    {
        ((ICollection<GameComponent>)_innerSet).Clear();
    }

    public bool Contains(GameComponent item)
    {
        return ((ICollection<GameComponent>)_innerSet).Contains(item);
    }

    public void CopyTo(GameComponent[] array, int arrayIndex)
    {
        ((ICollection<GameComponent>)_innerSet).CopyTo(array, arrayIndex);
    }

    public void ExceptWith(IEnumerable<GameComponent> other)
    {
        ((ISet<GameComponent>)_innerSet).ExceptWith(other);
    }

    public IEnumerator<GameComponent> GetEnumerator()
    {
        return ((IEnumerable<GameComponent>)_innerSet).GetEnumerator();
    }

    public void IntersectWith(IEnumerable<GameComponent> other)
    {
        ((ISet<GameComponent>)_innerSet).IntersectWith(other);
    }

    public bool IsProperSubsetOf(IEnumerable<GameComponent> other)
    {
        return ((ISet<GameComponent>)_innerSet).IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<GameComponent> other)
    {
        return ((ISet<GameComponent>)_innerSet).IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<GameComponent> other)
    {
        return ((ISet<GameComponent>)_innerSet).IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<GameComponent> other)
    {
        return ((ISet<GameComponent>)_innerSet).IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<GameComponent> other)
    {
        return ((ISet<GameComponent>)_innerSet).Overlaps(other);
    }

    public bool Remove(GameComponent item)
    {
        return ((ICollection<GameComponent>)_innerSet).Remove(item);
    }

    public bool SetEquals(IEnumerable<GameComponent> other)
    {
        return ((ISet<GameComponent>)_innerSet).SetEquals(other);
    }

    public void SymmetricExceptWith(IEnumerable<GameComponent> other)
    {
        ((ISet<GameComponent>)_innerSet).SymmetricExceptWith(other);
    }

    public void UnionWith(IEnumerable<GameComponent> other)
    {
        ((ISet<GameComponent>)_innerSet).UnionWith(other);
    }

    void ICollection<GameComponent>.Add(GameComponent item)
    {
        ((ICollection<GameComponent>)_innerSet).Add(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_innerSet).GetEnumerator();
    }

    #endregion
}
