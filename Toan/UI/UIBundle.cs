using System;

using Toan.ECS;
using Toan.ECS.Bundles;

namespace Toan.UI;

public readonly struct UIBundle : IBundle
{
    public Guid? ParentNodeId { get; init; } = null;

    public UIBundle() { }

    public void AddBundle(Entity entity)
    {
        entity
            .With(new UINode
            {
                ParentNode = ParentNodeId,
            });
    }
}
