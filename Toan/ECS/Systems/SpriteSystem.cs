using Microsoft.Xna.Framework;

using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.Rendering;

namespace Toan.ECS.Systems;

public class SpriteSystem : EntityRenderSystem
{
    public override WorldQuery<Sprite, Transform, Visible> Archetype => new();

    protected override void RenderEntity(Entity entity, Renderer renderer)
    {
        var sprite    = entity.Get<Sprite>();
        var transform = entity.Get<Transform>();

		Vector2 spriteSize   = new(sprite.Texture.Width, sprite.Texture.Height);
        Vector2 spriteOrigin = sprite.Origin * spriteSize;

        renderer.DrawTexture(new()
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

