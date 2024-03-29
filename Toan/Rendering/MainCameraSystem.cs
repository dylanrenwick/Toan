﻿using System;
using System.Linq;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Systems;

namespace Toan.Rendering;

public class MainCameraSystem : EntitySystem
{
    public override WorldQuery<Camera, MainCamera, Transform> Archetype => new();

    [RenderSystem]
    public void Render(World scene, Renderer renderer)
    {
        if (_entities.Count == 0) return;
        if (_entities.Count > 1) throw new Exception("Cannot have more than one MainCamera in the world!");

        var entity = scene.Entity(_entities.First());
        Camera camera = entity.Get<Camera>();
        Transform transform = entity.Get<Transform>();

        MainCamera.MainCameraEntity = camera;

        camera.WorldPosition = transform.Position;
        camera.WorldScale = renderer.RenderScale;
        camera.ScreenSize = renderer.ScreenSize;
        renderer.MainCamera = camera;
        entity.With(camera);
    }
}

