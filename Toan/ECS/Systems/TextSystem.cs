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
        var text      = entity.Components.Get<Text>();
        var transform = entity.Components.Get<Transform>();

        renderer.DrawString(new()
        {
            Font = text.Font,
            Position = transform.GlobalPosition,
            Text = text.Content,
            Color = text.Color,
        });
    }
}
