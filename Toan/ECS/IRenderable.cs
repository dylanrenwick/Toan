using Microsoft.Xna.Framework;

using Toan.Rendering;

namespace Toan.ECS;

public interface IRenderable
{
    public void Render(World scene, Renderer render, GameTime time);
}
