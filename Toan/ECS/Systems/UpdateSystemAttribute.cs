using System;

namespace Toan.ECS.Systems;

[AttributeUsage(AttributeTargets.Method)]
public class UpdateSystemAttribute : PrioritizedSystemAttribute
{
	public UpdateSystemAttribute(
		SystemExecutionPriority priority = SystemExecutionPriority.Standard
	) : base(priority) { }
}
