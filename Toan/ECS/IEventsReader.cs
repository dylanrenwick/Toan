using System;
using System.Collections.Generic;

namespace Toan.ECS;

public interface IEventsReader
{
    public IReadOnlySet<Guid> Added { get; }
    public IReadOnlySet<Guid> Changed { get; }
    public IReadOnlySet<Guid> Removed { get; }

    public bool WasAdded(Guid entityId);
    public bool WasChanged(Guid entityId);
    public bool WasRemoved(Guid entityId);
}
