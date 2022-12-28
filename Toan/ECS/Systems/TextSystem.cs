using Microsoft.Xna.Framework;

using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.Rendering;

namespace Toan.ECS.Systems;

public class TextSystem : EntityRenderSystem
{
    public override WorldQuery<Text, Transform, Visible> Archetype => new(); 

    protected override void RenderEntity(Entity entity, Renderer renderer, GameTime gameTime)
    {
        var text      = entity.Get<Text>();
        var transform = entity.Get<Transform>();

        renderer.DrawString(new()
        {
            Font = text.Font,
            Position = transform.Position,
            Text = text.Content,
            Color = text.Color,
        });
    }
}
