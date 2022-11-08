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

    /// <summary>
    /// Returns a set of IDs of entities that are close enough to the provided bounding box that they may intersect with it.
    /// </summary>
    /// <param name="posX">X position of the bounding box</param>
    /// <param name="posY">Y position of the bounding box</param>
    /// <param name="width">Width of the bounding box</param>
    /// <param name="height">Height of the bounding box</param>
    /// <returns></returns>
    public IReadOnlySet<Guid> GetPossibleCollisions(FloatRect boundingBox)
    {
        float cellOffsetX = boundingBox.Left % CellSize;
        float cellOffsetY = boundingBox.Top % CellSize;

        int cellOriginX = (int)Math.Floor(boundingBox.Left / CellSize);
        int cellOriginY = (int)Math.Floor(boundingBox.Top / CellSize);

        int cellBoundsX = (int)Math.Ceiling((cellOffsetX + boundingBox.Width) / CellSize);
        int cellBoundsY = (int)Math.Ceiling((cellOffsetY + boundingBox.Height) / CellSize);

        return GetEntitiesInBounds(new(cellOriginX, cellOriginY, cellBoundsX, cellBoundsY));
    }

    public IReadOnlySet<Guid> GetEntitiesInBounds(Rectangle boundingBox)
    {
        HashSet<Guid> results = new();

        for (int y = boundingBox.Y; y < boundingBox.Y + boundingBox.Height; y++)
        {
            for (int x = boundingBox.X; x < boundingBox.X + boundingBox.Width; x++)
            {
                results.UnionWith(this[x, y]);
            }
        }

        return results;
    }

    private Point WorldToCellPos(Vector2 worldPos)
        => new(
            x: (int)(worldPos.X / CellSize),
            y: (int)(worldPos.Y / CellSize)
        );
}
