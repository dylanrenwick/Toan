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
        var sprite    = entity.Get<Sprite>();
        var transform = entity.Get<Transform>();

		Vector2 spriteSize   = new(sprite.Texture.Width, sprite.Texture.Height);
        Vector2 spriteOrigin = sprite.Origin * spriteSize;

        renderer.Draw(new()
        {
            Color    = sprite.Color,
            Position = transform.GlobalPosition,
            Rotation = MathUtil.DegToRad(transform.GlobalRotation),
            Origin   = spriteOrigin,
            Scale    = transform.GlobalScale,
            Texture  = sprite.Texture,
        });
    }
}

