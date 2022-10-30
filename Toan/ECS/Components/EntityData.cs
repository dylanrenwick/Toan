namespace Toan.ECS.Components;

public struct EntityData : IComponent
{
	public required float CreatedAt { get; init; }

	public float LastModifiedAt { get; init; }
}
