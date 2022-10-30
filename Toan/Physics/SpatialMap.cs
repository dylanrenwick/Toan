using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Toan.Physics;

public class SpatialMap
{
    private readonly Dictionary<Point, HashSet<Guid>> _spatialTable = new();

    public required int CellSize { get; init; }

    public ISet<Guid> GetCloseColliders()
    {
        throw new NotImplementedException();
    }
}
