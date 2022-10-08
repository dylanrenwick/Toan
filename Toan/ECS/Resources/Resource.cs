using System;

namespace Toan.ECS.Resources;

public abstract class Resource
{
    public Guid Id { get; } = Guid.NewGuid();
}
