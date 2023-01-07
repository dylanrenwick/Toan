using System;
using System.Reflection;

namespace Toan.ECS.Systems;

public class SystemInfo
{
    public required object System { get; init; }
    public Type SystemType => System.GetType();

    public MethodInfo? UpdateSystem { get; init; }
    public MethodInfo? RenderSystem { get; init; }
    public MethodInfo? EntitySystem { get; init; }

    public PropertyInfo? EntityQuery { get; init; }
}
