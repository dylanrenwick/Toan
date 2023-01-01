using System;

namespace Toan.ECS.Systems;

[AttributeUsage(AttributeTargets.Method)]
public class EntitySystemAttribute : Attribute
{
	public string MemberName { get; }

	public EntitySystemAttribute(string memberName)
	{
		MemberName = memberName;
	}
}
