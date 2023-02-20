using System;

namespace Toan.ECS.Systems;

[AttributeUsage(AttributeTargets.Method)]
public class UpdateSystemAttribute : PrioritizedSystemAttribute
{
}
