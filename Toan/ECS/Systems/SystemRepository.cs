using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Toan.ECS.Query;
using Toan.Rendering;

namespace Toan.ECS.Systems;

public class SystemRepository
{
    private readonly Dictionary<SystemExecutionPriority, HashSet<PrioritizedSystemInfo>> _updateSystems = new()
    {
        [SystemExecutionPriority.VeryEarly] = new(),
        [SystemExecutionPriority.Early]     = new(),
        [SystemExecutionPriority.Standard]  = new(),
        [SystemExecutionPriority.Late]      = new(),
        [SystemExecutionPriority.VeryLate]  = new(),
    };
    private readonly Dictionary<SystemExecutionPriority, HashSet<PrioritizedSystemInfo>> _renderSystems = new()
    {
        [SystemExecutionPriority.VeryEarly] = new(),
        [SystemExecutionPriority.Early]     = new(),
        [SystemExecutionPriority.Standard]  = new(),
        [SystemExecutionPriority.Late]      = new(),
        [SystemExecutionPriority.VeryLate]  = new(),
    };

    private readonly HashSet<SystemInfo> _entitySystems = new();

    public void Add(SystemInfo system)
    {
        PrioritizedSystemInfo? updateInfo = system.UpdateInfo;
        PrioritizedSystemInfo? renderInfo = system.RenderInfo;

        if (updateInfo != null)
            _updateSystems[updateInfo.Value.Priority].Add(updateInfo.Value);
        if (renderInfo != null)
            _renderSystems[renderInfo.Value.Priority].Add(renderInfo.Value);

        if (system.EntitySystem != null && system.EntityQuery != null)
            _entitySystems.Add(system);
    }

    public void Update(World world, GameTime gameTime)
    {
        var updateParams = new object[] { world, gameTime };

        ExecuteInPriorityOrder(_updateSystems, systems => {
            foreach (var system in systems)
            {
                system.Method.Invoke(system.System, updateParams);
            }
        });
    }

    public void Render(World world, Renderer renderer, GameTime gameTime)
    {
        var renderParams = new object[] { world, renderer, gameTime };

        ExecuteInPriorityOrder(_renderSystems, systems => {
            foreach (var system in systems)
            {
                system.Method.Invoke(system.System, renderParams);
            }
        });
    }

	public void UpdateComponents(World world)
    {
        foreach (var system in _entitySystems)
        {
            if (system.EntitySystem == null || system.EntityQuery == null)
                continue;
            var query = (IWorldQuery)system.EntityQuery.GetValue(system.System)!;
            var entities = query.GetEntities(world);
            system.EntitySystem?.Invoke(system.System, new object[] { world, entities });
        }
    }

    private static void ExecuteInPriorityOrder(
        Dictionary<SystemExecutionPriority, HashSet<PrioritizedSystemInfo>> systems,
        Action<ISet<PrioritizedSystemInfo>> action
    ) {
        var execPriorities = Enum.GetValues<SystemExecutionPriority>();
        for(int i = 0; i < execPriorities.Length; i++)
        {
            action(systems[execPriorities[i]]);
        }
    }
}

