namespace Toan.ECS.Components;

public class EntityData : GameComponent
{
	public required float CreatedAt { get; init; }

	public float LastModifiedAt { get; init; }
}
