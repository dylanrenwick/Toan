using System;
using System.Collections.Generic;

namespace Toan.ECS;

public class Events : IEventsReader
{
    private HashSet<Guid> _added   = new();
    private HashSet<Guid> _changed = new();
    private HashSet<Guid> _removed = new();

    public IReadOnlySet<Guid> Added   => _added;
    public IReadOnlySet<Guid> Changed => _changed;
    public IReadOnlySet<Guid> Removed => _removed;

    public void AddEntity(Guid entityId)
        => _added.Add(entityId);
    public void ChangeEntity(Guid entityId)
        => _changed.Add(entityId);
    public void RemoveEntity(Guid entityId)
        => _removed.Add(entityId);

    public bool WasAdded(Guid entityId)
        => _added.Contains(entityId);
    public bool WasChanged(Guid entityId)
        => _changed.Contains(entityId);
    public bool WasRemoved(Guid entityId)
        => _removed.Contains(entityId);

    public void Clear()
    {
        _added.Clear();
        _changed.Clear();
        _removed.Clear();
    }
}

