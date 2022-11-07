using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Systems;

namespace Toan.Physics;

public class CircleCollisionSystem : EntityUpdateSystem
{
    public override WorldQuery<CircleCollider, Collider, Transform> Archetype => new();

    protected override void UpdateEntity(Entity entity, GameTime gameTime)
    {
        ref var collider = ref entity.Get<Collider>();
        ref var circle = ref entity.Get<CircleCollider>();
        ref var transform = ref entity.Get<Transform>();
        var circleOrigin = transform.Position + (collider.Origin * transform.Scale);
        var scaledRadius = circle.Radius * transform.Scale.X;

        var collisions = entity.Has<Collisions>()
			? entity.Get<Collisions>()
			: new Collisions();

        collisions.Clear();
        /*
         * Temporarily commented out collision code whilst spatial map is being implemented
         * 
         * This collision code iterates over every other collider in the world and checks for collisions.
         * 
        foreach ((var other, var otherCircle, var otherTransform) in Archetype.Enumerate(entity.World))
        {
            if (other.Id == entity.Id) continue;
            if (!circle.Mask.Has(otherCircle.Layer)) continue;

            var threshold = scaledRadius + (otherCircle.Radius * otherTransform.Scale.X);
            var otherOrigin = otherTransform.Position + (otherCircle.Origin * otherTransform.Scale);
            var distance = otherOrigin - circleOrigin;

            if (distance.LengthSquared() < threshold && distance.Length() < threshold)
            {
                distance.Normalize();
                collisions.Add(new Collision
                {
                    Other = other.Id,
                    CollisionNormal = distance,
                });
            }
        }
        */

		if (collisions.Count > 0 || !entity.Has<Collisions>())
		{
			entity.With(collisions);
		}
    }
}
