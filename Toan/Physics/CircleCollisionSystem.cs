using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Systems;

namespace Toan.Physics;

public class CircleCollisionSystem : EntityUpdateSystem
{
    public override WorldQuery<CircleCollider, Transform> Archetype => new();

    protected override void UpdateEntity(Entity entity, GameTime gameTime)
    {
        var circle = entity.Components.Get<CircleCollider>();
        var transform = entity.Components.Get<Transform>();
        var circleOrigin = transform.GlobalPosition + (circle.Origin * transform.GlobalScale);
        var scaledRadius = circle.Radius * transform.GlobalScale.X;

        var collisions = entity.Components.Has<Collisions>()
			? entity.Components.Get<Collisions>()
			: new Collisions();

        collisions.Clear();
        foreach ((var otherId, var otherCircle, var otherTransform) in Archetype.Enumerate(entity.World))
        {
            if (otherId == entity.Id) continue;
            if (!circle.Mask.Has(otherCircle.Layer)) continue;

            var threshold = scaledRadius + (otherCircle.Radius * otherTransform.GlobalScale.X);
            var otherOrigin = otherTransform.GlobalPosition + (otherCircle.Origin * otherTransform.GlobalScale);
            var distance = otherOrigin - circleOrigin;

            if (distance.LengthSquared() < threshold && distance.Length() < threshold)
            {
                distance.Normalize();
                collisions.Add(new Collision
                {
                    Other = otherId,
                    CollisionNormal = distance,
                });
            }
        }

		if (collisions.Count > 0 || !entity.Components.Has<Collisions>())
		{
			entity.With(collisions);
		}
    }
}
