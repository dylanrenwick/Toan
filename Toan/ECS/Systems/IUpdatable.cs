using Microsoft.Xna.Framework;

namespace Toan.ECS.Systems;

public interface IUpdateSystem : IGameSystem
{
    public void Update(World world, GameTime time);
}
