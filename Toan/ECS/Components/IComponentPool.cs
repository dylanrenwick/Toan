using System;

namespace Toan.ECS.Components;
public interface IComponentPool
{
    public int Count { get; }
    
    public void Add(Guid entityId, in object component);
    public bool Remove(Guid entityId);
    public bool HasEntity(Guid entityId);
}
