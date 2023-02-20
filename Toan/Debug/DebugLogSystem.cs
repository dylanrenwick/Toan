using System;
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
    private const bool SHOW_LOG = false;

    public override WorldQuery<DebugLog, Text> Archetype => new();

    protected override void UpdateEntity(Entity entity, GameTime gameTime)
    {
        bool visible = entity.Has<Visible>();

        var debugState = entity.World.Resource<DebugState>();
        if (!debugState.DebugActive)
        {
            if (visible) RemoveVisible(entity);
            return;
        }

        var text = entity.Get<Text>();

        if (!visible)
            AddVisible(entity);

        // TODO: Automate this toggle
        if (SHOW_LOG)
        {
            var debugLog = entity.Get<DebugLog>();
            Guid logId = debugLog.LogResourceID;
            TextLog log = entity.World.Resource<TextLog>(logId);

            text.Content = log.GetEntries(debugLog.EntryCount);
        }
        else
        {
            text.Content = GetDiagnosticStats(entity.World);
        }

        entity.With(text);
    }

    protected string GetDiagnosticStats(World world)
    {
        InputState input = world.Resource<InputState>();
        Camera camera = MainCamera.MainCameraEntity;

        StringBuilder sb = new();
        sb.AppendLine($"Mouse Pos: {input.MousePosition}");
        sb.AppendLine($"World Pos: {camera.ScreenToWorld(input.MousePosition.ToVector2())}");

        return sb.ToString();
    }

    protected void RemoveVisible(Entity entity)
    {
        entity.Without<Visible>();
    }

    protected void AddVisible(Entity entity)
    {
        entity.With(new Visible());
    }
}

