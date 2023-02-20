using System;

namespace Toan.ECS.Systems;

[AttributeUsage(AttributeTargets.Method)]
public class RenderSystemAttribute : PrioritizedSystemAttribute
{
	public RenderSystemAttribute(
		SystemExecutionPriority priority = SystemExecutionPriority.Standard
	) : base(priority) { }
}
