using System;
using System.Collections.Generic;
using System.Linq;

namespace Toan.ECS;

public class Events : IEventsReader
{
    private readonly HashSet<Guid> _added   = new();
    private readonly Dictionary<Guid, HashSet<Type>> _changed = new();
    private readonly HashSet<Guid> _removed = new();

    public IReadOnlySet<Guid> Added   => _added;
    public IReadOnlySet<Guid> Changed => _changed.Keys.ToHashSet();
    public IReadOnlySet<Guid> Removed => _removed;

    public void AddEntity(Guid entityId)
        => _added.Add(entityId);
    public void ChangeEntity(Guid entityId)
        => GetChangedTypes(entityId);
    public void ChangeEntity<T>(Guid entityId)
        where T : struct
    {
        ISet<Type> changedTypes = GetChangedTypes(entityId);
        changedTypes.Add(typeof(T));
    }
    public void RemoveEntity(Guid entityId)
        => _removed.Add(entityId);

    public bool WasAdded(Guid entityId)
        => _added.Contains(entityId);
    public bool WasChanged(Guid entityId)
        => _changed.ContainsKey(entityId);
    public bool WasChanged<T>(Guid entityId)
        where T : struct
    => WasChanged(entityId) && _changed[entityId].Contains(typeof(T));
    public bool WasRemoved(Guid entityId)
        => _removed.Contains(entityId);

    public void Clear()
    {
        _added.Clear();
        _changed.Clear();
        _removed.Clear();
    }

    private ISet<Type> GetChangedTypes(Guid entityId)
    {
        if (!_changed.ContainsKey(entityId))
            _changed.Add(entityId, new HashSet<Type>());

        return _changed[entityId];
    }
}

