using System;
using System.Collections.Generic;
using System.Linq;

namespace Toan.ECS;

internal class VirtualEntity : BaseEntity
{
    private Dictionary<Type, ValueType> _components = new();
    private bool _isMade = false;

    private Guid _id = Guid.Empty;

    public override required Guid Id
    {
        get => _id;
        init => _id = value;
    }

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
        Type keyType = typeof(T);
        if (_components.ContainsKey(keyType))
            _components[keyType] = component;
        else
            _components.Add(keyType, component);
        return this;
    }
    public override IEntity Without<T>()
    {
        _components.Remove(typeof(T));
        return this;
    }
    public override Guid Make()
    {
        do
        {
            if (World.HasEntity(_id))
            {
                Entity worldEntity = World.Entity(_id);
                if (worldEntity.Count() >= 1)
                {
                    _id = Guid.NewGuid();
                    continue;
                }
            }
            break;
        } while (true);

        _isMade = true;
        return base.Make();
    }

    ~VirtualEntity()
    {
        if (!_isMade)
            World.Log.Warn($"Entity {Id} was not made before being destroyed");
    }
}
