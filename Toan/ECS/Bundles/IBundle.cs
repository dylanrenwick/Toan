using System;

using Toan.ECS.Components;

namespace Toan.ECS.Bundles;
public interface IBundle
{
    public void AddBundle(Guid entityId, ComponentRepository componentRepo);

    public static void AddIfNew<T>(Guid entityId, ComponentRepository componentRepo, T component)
        where T : struct
    {
        if (!componentRepo.Has<T>(entityId))
            componentRepo.Add<T>(entityId, component);
    }
}
