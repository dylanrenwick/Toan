using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Toan.ECS.Query;
using Toan.Rendering;

namespace Toan.ECS.Systems;

public class SystemRepository
{
    private readonly Dictionary<SystemExecutionPriority, HashSet<SystemInfo>> _systems = new()
    {
        [SystemExecutionPriority.VeryEarly] = new(),
        [SystemExecutionPriority.Early]     = new(),
        [SystemExecutionPriority.Standard]  = new(),
        [SystemExecutionPriority.Late]      = new(),
        [SystemExecutionPriority.VeryLate]  = new(),
    };

    public void Add(
        SystemInfo system,
        SystemExecutionPriority priority = SystemExecutionPriority.Standard
    )
    => _systems[priority].Add(system);

    public void Update(World world, GameTime gameTime)
    {
        var updateParams = new object[] { world, gameTime };

        ExecuteInPriorityOrder(system =>
            system.UpdateSystem?.Invoke(system.System, updateParams)
        );
    }

    public void Render(World world, Renderer renderer, GameTime gameTime)
    {
        var renderParams = new object[] { world, renderer, gameTime };

        ExecuteInPriorityOrder(system =>
            system.RenderSystem?.Invoke(system.System, renderParams)
        );
    }

	public void UpdateComponents(World world)
    {
        ExecuteInPriorityOrder(system => {
            if (system.EntitySystem == null || system.EntityQuery == null)
                return;
            var query = (IWorldQuery)system.EntityQuery.GetValue(system.System)!;
            var entities = query.GetEntities(world);
            system.EntitySystem?.Invoke(system.System, new object[] { world, entities });
        });
    }

    private void ExecuteInPriorityOrder(Action<SystemInfo> action)
    {
        var execPriorities = Enum.GetValues<SystemExecutionPriority>();
        for(int i = 0; i < execPriorities.Length; i++)
        {
            foreach (SystemInfo system in _systems[execPriorities[i]])
            {
                action(system);
            }
        }
    }
}

