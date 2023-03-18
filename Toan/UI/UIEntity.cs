using System;

using Toan.ECS;
using Toan.ECS.Bundles;

namespace Toan.UI;

public class UIEntity : Entity, IEntity
{
    public new UIEntity With<T>()
        where T : struct
    => (UIEntity)base.With<T>();
    public new UIEntity With<T>(T component)
        where T : struct
    => (UIEntity)base.With(component);

    public new UIEntity WithIfNew<T>()
        where T : struct
    => (UIEntity)base.WithIfNew<T>();
    public new UIEntity WithIfNew<T>(T component)
        where T : struct
    => (UIEntity)base.WithIfNew(component);

    public new UIEntity WithBundle(IBundle bundle)
        => (UIEntity)base.WithBundle(bundle);

    public new UIEntity Without<T>()
        where T : struct
    => (UIEntity)base.Without<T>();

    public UIEntity CreateChild()
    {
        return World.CreateUI(Id);
    }

    public UIEntity WithChild(Action<UIEntity> buildChild)
    {
        UIEntity childBuilder = World.CreateUI(Id);
        buildChild(childBuilder);

        return this;
    }
}
