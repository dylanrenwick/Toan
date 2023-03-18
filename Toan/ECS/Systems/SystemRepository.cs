using System;
using System.Collections.Generic;
using System.Linq;

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

    public int Count
        => _updateSystems.Values.Sum(set => set.Count)
         + _renderSystems.Values.Sum(set => set.Count)
         + _entitySystems.Count;

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

    public void Update(World world)
    {
        var updateParams = new object[] { world };

        ExecuteInPriorityOrder(_updateSystems, systems => {
            if (world.IsDirty)
                UpdateComponents(world);
            foreach (var system in systems)
            {
                system.Method.Invoke(system.System, updateParams);
            }
        });

        if (world.IsDirty)
            UpdateComponents(world);
    }

    public void Render(World world, Renderer renderer)
    {
        var renderParams = new object[] { world, renderer};

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

