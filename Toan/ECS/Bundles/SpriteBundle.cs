using System.Collections.Generic;
using Toan.ECS.Components;

namespace Toan.ECS.Bundles;

public readonly struct SpriteBundle : IBundle
{
    public required bool Visible { get; init; }

    public Sprite Sprite { get; init; }

    public void AddBundle(Entity entity)
    {
        entity.WithIfNew(Sprite);
        if (Visible)
            entity.With<Visible>();
    }

    public HashSet<IComponent> FlattenBundle()
    {
        HashSet<IComponent> componentSet = new()
        {
            Sprite,
        };
        if (Visible)
            componentSet.Add(new Visible());

        return componentSet;
    }
}
