using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Query;
using Toan.ECS.Resources;
using Toan.ECS.Systems;
using Toan.Input;

namespace Toan.Debug;

public class DebugLogSystem : EntityUpdateSystem
{
    public override IWorldQuery Archetype => new WorldQuery<DebugLog, Text>();

    protected override void UpdateEntity(Entity entity, GameTime gameTime)
    {
        DebugLog debugLog = entity.Get<DebugLog>();
        Text text = entity.Get<Text>();
        bool visible = entity.Has<Visible>();

        var input = entity.World.Resource<InputState>();
        if (input.KeyPressed(Keys.OemTilde))
        {
            visible = !visible;
            if (visible) entity.With(new Visible());
            else entity.Without<Visible>();
        }

        if (visible)
        {
            Guid logId = debugLog.LogResourceID;
            TextLog log = entity.World.Resource<TextLog>(logId);

            text.Content = log.GetEntries(debugLog.EntryCount);
        }
    }
}

