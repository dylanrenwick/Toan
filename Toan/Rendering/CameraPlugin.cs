using Toan.ECS;

namespace Toan.Rendering;

public class CameraPlugin : Plugin
{
    public override void Build(World world)
    {
        world.AddSystem<MainCameraSystem>();

        world.CreateEntity(new(0, 0))
            .With(new Camera
            {
                Anchor = CameraAnchor.Center,
            }).With(new MainCamera());
    }
}

