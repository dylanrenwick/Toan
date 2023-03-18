using Toan.ECS.Components;

namespace Toan.ECS.Bundles;

public struct SpriteBundle : IBundle
{
    public required bool Visible { get; init; }

    public Sprite Sprite { get; init; }

    public void AddBundle(IEntity entity)
    {
        entity.WithIfNew(Sprite);
        if (Visible)
            entity.With<Visible>();
    }
}
