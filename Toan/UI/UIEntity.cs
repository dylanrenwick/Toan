using System;

using Toan.ECS;

namespace Toan.UI;

public struct UIEntity : IEntityBuilder<UIEntity>
{
    private readonly World _world;
    private readonly Entity _entity;

    public Guid Id => _entity.Id;

    public UIEntity(World world, Entity entity)
    {
        _world = world;
        _entity = entity;
    }

    public UIEntity With<T>()
        where T : struct
    {
        _entity.With<T>();
        return this;
    }

    public UIEntity With<T>(T component)
        where T : struct
    {
        _entity.With(component);
        return this;
    }

    public UIEntity Without<T>()
        where T : struct
    {
        _entity.Without<T>();
        return this;
    }    

    public bool Has<T>()
        where T : struct
    => _entity.Has<T>();

    public UIEntity CreateChild()
    {
        return _world.CreateUI(_entity.Id);
    }

    public UIEntity WithChild(Action<UIEntity> buildChild)
    {
        UIEntity childBuilder = _world.CreateUI(_entity.Id);
        buildChild(childBuilder);

        return this;
    }
}
