using System;

using Toan.ECS.Components;

namespace Toan.ECS.Bundles;

public struct SpriteBundle : IBundle
{
    public required bool Visible { get; init; }

    public Sprite Sprite { get; init; }

    public void AddBundle(Guid entityId, ComponentRepository componentRepo)
    {
        IBundle.AddIfNew(entityId, componentRepo, Sprite);
        if (Visible)
            IBundle.AddIfNew<Visible>(entityId, componentRepo, default);
    }
}
