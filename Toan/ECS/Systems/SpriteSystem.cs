using Microsoft.Xna.Framework;

using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.Rendering;

namespace Toan.ECS.Systems;

public class SpriteSystem : EntityRenderSystem
{
    public override WorldQuery<Sprite, Transform, Visible> Archetype => new();

    protected override void RenderEntity(Entity entity, Renderer renderer, GameTime gameTime)
    {
        ref var sprite    = ref entity.Get<Sprite>();
        ref var transform = ref entity.Get<Transform>();

		Vector2 spriteSize   = new(sprite.Texture.Width, sprite.Texture.Height);
        Vector2 spriteOrigin = sprite.Origin * spriteSize;

        renderer.Draw(new()
        {
            Color    = sprite.Color,
            Position = transform.Position,
            Rotation = MathUtil.DegToRad(transform.Rotation),
            Origin   = spriteOrigin,
            Scale    = transform.Scale,
            Texture  = sprite.Texture,
        });
    }
}

