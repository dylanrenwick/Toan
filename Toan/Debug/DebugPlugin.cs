using System;

using Microsoft.Xna.Framework.Graphics;

using Toan.ECS;
using Toan.ECS.Components;
using Toan.ECS.Resources;
using Toan.Logging;
using Toan.Logging.Color;

namespace Toan.Debug;

public class DebugPlugin : Plugin
{
	public bool AutoApply { get; init; } = false;

    public override void Build(World world)
    {
        ContentServer content = world.Resource<ContentServer>();
        SpriteFont font = content.Load<SpriteFont>("Font");

        world.AddResource(new DebugState());

		TextLog debugLog = new();
        Guid logId = world.AddResource(debugLog);
		world.Log.Destinations.Add(
			new ColoredDestination
			{
				Color = new NoneColorConverter(),
				Destination = new TextLogDestination()
				{
					TextLog = debugLog
				}
			}
		);

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

