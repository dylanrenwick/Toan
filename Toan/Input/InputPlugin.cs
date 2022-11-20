using Toan.ECS;

namespace Toan.Input;

public class InputPlugin : Plugin
{
    public override void Build(World world)
    {
        world.AddResource(new InputState());

        world.Systems()
            .Add<InputSystem>()
            .Add<PlayerInputSystem>();
    }
}

