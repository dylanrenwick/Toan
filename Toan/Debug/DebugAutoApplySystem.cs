using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Systems;
using Toan.Physics;

namespace Toan.Debug;

public class DebugAutoApplySystem : EntityUpdateSystem
{
	public override WorldQuery<Collider, Transform, Added> Archetype => new();

	protected override void UpdateEntity(Entity entity, GameTime gameTime)
	{
		if (!entity.Has<Debug>()) entity.With<Debug>();
	}
}

