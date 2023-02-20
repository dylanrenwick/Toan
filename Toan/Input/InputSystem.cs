using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Toan.ECS;
using Toan.ECS.Systems;

namespace Toan.Input;

public class InputSystem
{
    [UpdateSystem]
    public void Update(World scene, GameTime time)
    {
        var inputResource = scene.Resource<InputState>();
        var keyboardState = Keyboard.GetState();
        var mouseState = Mouse.GetState();

        inputResource.Update(keyboardState, mouseState);
    }
}

