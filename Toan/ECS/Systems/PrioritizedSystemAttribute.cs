using System;

namespace Toan.ECS.Systems;

public class PrioritizedSystemAttribute : Attribute
{
    public PrioritizedSystemAttribute(
        SystemExecutionPriority priority = SystemExecutionPriority.Standard
    ) {
        Priority = priority;
    }

    public SystemExecutionPriority Priority { get; }
}
