using System;
using System.Collections.Generic;

using Toan.ECS;
using Toan.ECS.Bundles;
using Toan.ECS.Components;

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

    public HashSet<IComponent> FlattenBundle()
    {
        return new()
        {
            new UINode
            {
                ParentNode = ParentNodeId,
            },
        };
    }
}
