using System;

using Microsoft.Xna.Framework;

namespace Toan.Physics;

public readonly struct Collision
{
    public required Guid Other { get; init; }
    public required Vector2 CollisionNormal { get; init; }
}
