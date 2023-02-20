using System;
using System.Dynamic;
using System.Reflection;

namespace Toan.ECS.Systems;

public readonly struct SystemInfo
{
    public required object System { get; init; }
    public Type SystemType => System.GetType();

    public MethodInfo? UpdateSystem { get; init; }
    public MethodInfo? RenderSystem { get; init; }
    public MethodInfo? EntitySystem { get; init; }

    public PropertyInfo? EntityQuery { get; init; }

    public PrioritizedSystemInfo? UpdateInfo
        => BuildSystemInfo<UpdateSystemAttribute>(SystemType, UpdateSystem);

    public PrioritizedSystemInfo? RenderInfo
        => BuildSystemInfo<RenderSystemAttribute>(SystemType, RenderSystem);

    private static PrioritizedSystemInfo? BuildSystemInfo<TAttribute>(Type systemType, MethodInfo? method)
        where TAttribute : PrioritizedSystemAttribute
    {
        if (method == null)
            return null;

        TAttribute? attribute = method.GetCustomAttribute<TAttribute>();

        return new()
        {
            Priority = attribute == null
                ? SystemExecutionPriority.Standard
                : attribute.Priority,
            SystemType = systemType,
            System = method,
        };
    }
}

public readonly struct PrioritizedSystemInfo
{
    public Type SystemType { get; init; }
    public MethodInfo System { get; init; }
    public SystemExecutionPriority Priority { get; init; }
}
