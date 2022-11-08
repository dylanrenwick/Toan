using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Toan.ECS.Resources;

namespace Toan.Physics;

public class SpatialMap : Resource
{
    private readonly Dictionary<Point, HashSet<Guid>> _spatialTable = new();

    public required int CellSize { get; init; }

    public IReadOnlySet<Guid> this[float x, float y]
    {
        get
        {
            var cellPos = WorldToCellPos(new(x, y));
            return _spatialTable.ContainsKey(cellPos)
                ? _spatialTable[cellPos]
                : new HashSet<Guid>();
        }
    }

    private Point WorldToCellPos(Vector2 worldPos)
        => new(
            x: (int)(worldPos.X / CellSize),
            y: (int)(worldPos.Y / CellSize)
        );
}
