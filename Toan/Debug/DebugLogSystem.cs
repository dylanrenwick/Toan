using System;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Resources;
using Toan.ECS.Systems;
using Toan.Input;
using Toan.Rendering;

namespace Toan.Debug;

public class DebugLogSystem : EntityUpdateSystem
{
    public override WorldQuery<DebugLog, Text> Archetype => new();

    protected override void UpdateEntity(Entity entity, GameTime gameTime)
    {
        bool visible = entity.Has<Visible>();

        var debugState = entity.World.Resource<DebugState>();
        if (!debugState.HasTextDisplay)
        {
            if (visible) entity.Without<Visible>();
            return;
        }

        var text = entity.Get<Text>();

        if (!visible)
            entity.With<Visible>();

        text.Content = debugState.DisplayState switch
        {
            DebugDisplayState.Stats => GetDiagnosticStats(entity.World),
            DebugDisplayState.Log   => GetLogContents(entity),
            _                       => throw new UnreachableException()
        };

        entity.With(text);
    }

    protected string GetLogContents(Entity entity)
    {
        var debugLog = entity.Get<DebugLog>();
        Guid logId = debugLog.LogResourceID;
        TextLog log = entity.World.Resource<TextLog>(logId);

        return log.GetEntries(debugLog.EntryCount);
    }

    protected string GetDiagnosticStats(World world)
    {
        InputState input = world.Resource<InputState>();
        Camera camera = MainCamera.MainCameraEntity;
        Vector2 mouseWorldPos = camera.ScreenToWorld(input.MousePosition.ToVector2());

        StringBuilder sb = new();
        sb.AppendLine($"Mouse Pos: {input.MousePosition}");
        sb.AppendLine($"World Pos: {mouseWorldPos}");

        return sb.ToString();
    }
}

