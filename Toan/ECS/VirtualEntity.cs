using System;
using System.Collections.Generic;
using System.Linq;

namespace Toan.ECS;

internal class VirtualEntity : BaseEntity
{
    private Dictionary<Type, ValueType> _components = new();

    public override T Get<T>()
        => (T)_components[typeof(T)];
    public override ValueType[] GetAll()
        => _components.Values.ToArray();
    public override bool Has<T>()
        => _components.ContainsKey(typeof(T));
    public override int Count()
        => _components.Count;
    public override IEntity With<T>(T component)
    {
        _components.Add(typeof(T), component);
        return this;
    }
    public override IEntity Without<T>()
    {
        _components.Remove(typeof(T));
        return this;
    }
}
