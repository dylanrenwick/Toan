using System;

using Toan.ECS;

namespace Toan.UI;

public class UIPlugin : Plugin
{
    public override void Build(World world)
    {
        Guid canvasRoot = world.CreateUI()
            .Id;

        UICanvas canvas = new(canvasRoot);

        world.AddResource(canvas);
    }
}
