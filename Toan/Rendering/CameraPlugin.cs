using Microsoft.Xna.Framework;
using Toan.ECS;

namespace Toan.Rendering;

public class CameraPlugin : Plugin
{
    public override void Build(World world)
    {
        world.Systems()
            .Add<MainCameraSystem>();

        world.CreateEntity(new Vector2(0, 0))
            .With(new Camera
            {
                Anchor = CameraAnchor.Center,
            }).With(new MainCamera());
    }
}

