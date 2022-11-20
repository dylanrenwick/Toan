using Microsoft.Xna.Framework;

using Toan.Rendering;

namespace Toan.ECS.Systems;

public interface IRenderSystem : IGameSystem
{
    public void Render(World scene, Renderer render, GameTime time);
}
