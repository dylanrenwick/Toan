using System.Collections.Generic;

using Toan.ECS.Components;

namespace Toan.ECS.Bundles;

public interface IBundle
{
    public void AddBundle(Entity entity);

    public HashSet<IComponent> FlattenBundle();
}
