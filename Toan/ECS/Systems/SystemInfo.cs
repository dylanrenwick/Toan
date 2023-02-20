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
        => BuildSystemInfo<UpdateSystemAttribute>(UpdateSystem);

    public PrioritizedSystemInfo? RenderInfo
        => BuildSystemInfo<RenderSystemAttribute>(RenderSystem);

    private PrioritizedSystemInfo? BuildSystemInfo<TAttribute>(MethodInfo? method)
        where TAttribute : PrioritizedSystemAttribute
    {
        if (method == null)
            return null;

        TAttribute? attribute = method.GetCustomAttribute<TAttribute>();

        return new()
        {
            Method = method,
            Priority = attribute == null
                ? SystemExecutionPriority.Standard
                : attribute.Priority,
            System = System,
            SystemType = SystemType,
        };
    }
}

public readonly struct PrioritizedSystemInfo
{
    public required object System { get; init; }
    public Type SystemType { get; init; }
    public MethodInfo Method { get; init; }
    public SystemExecutionPriority Priority { get; init; }
}
