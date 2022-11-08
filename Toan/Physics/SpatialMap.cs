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
    public IReadOnlySet<Guid> GetPossibleCollisions(float posX, float posY, float width, float height)
    {
        float cellOffsetX = posX % CellSize;
        float cellOffsetY = posY % CellSize;

        int cellOriginX = (int)Math.Floor(posX / CellSize);
        int cellOriginY = (int)Math.Floor(posY / CellSize);

        int cellBoundsX = (int)Math.Ceiling((cellOffsetX + width) / CellSize);
        int cellBoundsY = (int)Math.Ceiling((cellOffsetY + height) / CellSize);

        return GetEntitiesInBounds(cellOriginX, cellOriginY, cellBoundsX, cellBoundsY);
    }

    public IReadOnlySet<Guid> GetEntitiesInBounds(int posX, int posY, int width, int height)
    {
        posX   = Math.Min(posX, posX + width);
        posY   = Math.Min(posY, posY + height);
        width  = Math.Abs(width);
        height = Math.Abs(height);

        HashSet<Guid> results = new();

        for (int y = posY; y < posY + height; y++)
        {
            for (int x = posX; x < posX + width; x++)
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
