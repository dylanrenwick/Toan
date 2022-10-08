using System;

namespace Toan.ECS.Components;

public abstract class GameComponent
{
    public Guid Id { get; } = Guid.NewGuid();
}
