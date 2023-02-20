using System;

namespace Toan.ECS.Systems;

[AttributeUsage(AttributeTargets.Method)]
public class RenderSystemAttribute : PrioritizedSystemAttribute
{
}
