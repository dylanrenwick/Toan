using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Systems;

namespace Toan.Input;

public class PlayerInputSystem : EntityUpdateSystem
{
    public override IWorldQuery Archetype => new WorldQuery<Motor, PlayerInput>();

    protected override void UpdateEntity(Entity entity, GameTime time)
    {
        Motor motor = entity.Components.Get<Motor>();

        var input = entity.World.Resource<InputState>();
        Vector2 inputDir = Vector2.Zero;

        if (input.KeyDown(Keys.A)) inputDir -= new Vector2(1, 0);
        if (input.KeyDown(Keys.D)) inputDir += new Vector2(1, 0);
        if (input.KeyDown(Keys.W)) inputDir -= new Vector2(0, 1);
        if (input.KeyDown(Keys.S)) inputDir += new Vector2(0, 1);

        motor.Input = inputDir;
    }
}

