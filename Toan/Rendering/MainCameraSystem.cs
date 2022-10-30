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
    public override WorldQuery<Camera, MainCamera, Transform> Archetype => new();

    public override void Render(World scene, Renderer renderer, GameTime gameTime)
    {
        if (_entities.Count == 0) return;
        if (_entities.Count > 1) throw new Exception("Cannot have more than one MainCamera in the world!");

        var entity = scene.Entity(_entities.First());
        Camera camera = entity.Get<Camera>();
        Transform transform = entity.Get<Transform>();

        camera.WorldPosition = transform.Position;
        renderer.MainCamera = camera;
    }

    protected override void RenderEntity(Entity entity, Renderer renderer, GameTime gameTime) { }
}

