using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Toan.ECS;
using Toan.ECS.Systems;
using Toan.Input;

namespace Toan.Debug;

public class DebugToggleSystem : IGameSystem, IUpdatable
{
    public void Update(World world, GameTime time)
    {
        var debug = world.Resource<DebugState>();
        var input = world.Resource<InputState>();

        if (input.KeyPressed(Keys.OemTilde))
        {
            debug.DebugActive = !debug.DebugActive;
        }
    }
}

