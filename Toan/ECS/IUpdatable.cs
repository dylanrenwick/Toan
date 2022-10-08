using Microsoft.Xna.Framework;

namespace Toan.ECS;

public interface IUpdatable
{
    public void Update(World world, GameTime time);
}
