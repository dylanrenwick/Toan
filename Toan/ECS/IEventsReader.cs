using System;

namespace Toan.ECS;

public interface IEventsReader
{
    public bool WasAdded(Guid entityId);
    public bool WasChanged(Guid entityId);
}
