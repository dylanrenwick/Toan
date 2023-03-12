using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Toan.ECS;
using Toan.ECS.Systems;
using Toan.Input;

namespace Toan.Debug;

public class DebugToggleSystem
{
    [UpdateSystem]
    public void Update(World world, GameTime time)
    {
        var debug = world.Resource<DebugState>();
        var input = world.Resource<InputState>();

        if (input.KeyPressed(Keys.OemTilde))
        {
            var displayStateCount = Enum.GetValues(typeof(DebugDisplayState)).Length;
            var next = (int)debug.DisplayState + 1;
            debug.DisplayState = (DebugDisplayState)(next % displayStateCount);
        }
    }
}

