using System;
using System.Collections.Generic;

namespace Toan.ECS;

public class Events : IEventsReader
{
    private HashSet<Guid> _added = new();
    private HashSet<Guid> _changed = new();

    public void AddEntity(Guid entityId)
        => _added.Add(entityId);
    public void ChangeEntity(Guid entityId)
        => _changed.Add(entityId);

    public bool WasAdded(Guid entityId)
        => _added.Contains(entityId);
    public bool WasChanged(Guid entityId)
        => _changed.Contains(entityId);

    public void Clear()
    {
        _added.Clear();
        _changed.Clear();
    }
}

