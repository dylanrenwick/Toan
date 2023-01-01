using Microsoft.Xna.Framework;

using Toan.Rendering;

namespace Toan.ECS.Systems;

public abstract class RenderSystem : IGameSystem
{
    [RenderSystem]
    public abstract void Render(World scene, Renderer render, GameTime time);
}
