using System;

using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Resources;
using Toan.ECS.Systems;

namespace Toan.Debug;

public class DebugLogSystem : EntityUpdateSystem
{
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

        var debugLog = entity.Get<DebugLog>();
        var text     = entity.Get<Text>();

        if (!visible)
            AddVisible(entity);

        Guid logId = debugLog.LogResourceID;
        TextLog log = entity.World.Resource<TextLog>(logId);

        text.Content = log.GetEntries(debugLog.EntryCount);
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

