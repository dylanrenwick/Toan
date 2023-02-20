using System;

namespace Toan.ECS.Systems;

public abstract class PrioritizedSystemAttribute : Attribute
{
    public PrioritizedSystemAttribute(
        SystemExecutionPriority priority
    ) {
        Priority = priority;
    }

    public SystemExecutionPriority Priority { get; }
}
