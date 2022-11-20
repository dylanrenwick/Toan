using System;

using Microsoft.Xna.Framework.Graphics;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Resources;

namespace Toan.Debug;

public class DebugPlugin : Plugin
{
	public bool AutoApply { get; init; } = false;

    public override void Build(World world)
    {
        ContentServer content = world.Resource<ContentServer>();
        SpriteFont font = content.Load<SpriteFont>("Font");

        world.AddResource(new DebugState());
        Guid logId = world.AddResource(new TextLog());

		world.Systems()
			.Add<DebugToggleSystem>()
			.Add<DebugRenderSystem>()
			.Add<DebugLogSystem>();

		if (AutoApply)
			world.Systems()
				.Add<DebugAutoApplySystem>();

		world.CreateEntity(new(10.0f, 5.0f))
			.With(new DebugLog
			{
				LogResourceID = logId,
				EntryCount = 32,
			})
			.With(new Text { Font = font });
	}
}

