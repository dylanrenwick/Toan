using System;

using Toan.ECS.Components;

namespace Toan.UI;

public struct UINode : IComponent
{
    public Guid? ParentNode;
}
