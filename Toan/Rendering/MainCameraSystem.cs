using System;
using System.Linq;

using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Systems;

namespace Toan.Rendering;

public class MainCameraSystem : EntityRenderSystem
{
    public override IWorldQuery Archetype => new WorldQuery<Camera, MainCamera, Transform>();

    public override void Render(World scene, Renderer renderer, GameTime gameTime)
    {
        if (_entities.Count == 0) return;
        if (_entities.Count > 1) throw new Exception("Cannot have more than one MainCamera in the world!");

        var components = scene.Components(_entities.First());
        Camera camera = components.Get<Camera>();
        Transform transform = components.Get<Transform>();

        camera.WorldPosition = transform.GlobalPosition;
        renderer.MainCamera = camera;
    }

    protected override void RenderEntity(Entity entity, Renderer renderer, GameTime gameTime) { }
}

