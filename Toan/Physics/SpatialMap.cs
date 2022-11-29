using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Toan.ECS;
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
            return _spatialTable.TryGetValue(cellPos, out var cell)
                ? cell
                : new HashSet<Guid>();
        }
    }

    public bool Add(Entity entity)
        => Add(entity, ref entity.Get<Collider>());

    public bool Add(Entity entity, ref Collider collider)
        => Add(entity.Id, CollisionHelper.GetColliderBoundingBox(entity, ref collider));

    public bool Add(Guid entityId, FloatRect boundingBox)
    {
        var cellBounds = CellBoundsFromRealBounds(boundingBox);
        var cells = GetCellsInBounds(cellBounds);
        bool success = false;

        foreach (Point cell in cells)
        {
            // Only require any one success
            success |= AddToCell(entityId, cell);
        }

        return success;
    }

    public void Remove(Guid entityId)
    {
        foreach (var kvp in _spatialTable)
        {
            kvp.Value.Remove(entityId);
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
        => GetEntitiesInBounds(CellBoundsFromRealBounds(boundingBox));

    public IReadOnlySet<Guid> GetEntitiesInBounds(Rectangle boundingBox)
    {
        HashSet<Guid> results = new();

        foreach (Point cell in GetCellsInBounds(boundingBox))
        {
            results.UnionWith(this[cell.X, cell.Y]);
        }

        return results;
    }

    private bool AddToCell(Guid guid, Point cell)
    {
        if (!_spatialTable.ContainsKey(cell))
            _spatialTable.Add(cell, new HashSet<Guid>());
        return _spatialTable[cell].Add(guid);
    }

    private static IReadOnlySet<Point> GetCellsInBounds(Rectangle boundingBox)
    {
        HashSet<Point> results = new();

        for (int y = boundingBox.Y; y < boundingBox.Y + boundingBox.Height; y++)
        {
            for (int x = boundingBox.X; x < boundingBox.X + boundingBox.Width; x++)
            {
                results.Add(new(x, y));
            }
        }

        return results;
    }

    private Rectangle CellBoundsFromRealBounds(FloatRect boundingBox)
    {
        float cellOffsetX = boundingBox.Left % CellSize;
        float cellOffsetY = boundingBox.Top % CellSize;

        return new(
            x      : (int)Math.Floor(boundingBox.Left / CellSize),
            y      : (int)Math.Floor(boundingBox.Top / CellSize),
            width  : (int)Math.Ceiling((cellOffsetX + boundingBox.Width) / CellSize),
            height : (int)Math.Ceiling((cellOffsetY + boundingBox.Height) / CellSize)
        );
    }

    private Point WorldToCellPos(Vector2 worldPos)
        => new(
            x: (int)(worldPos.X / CellSize),
            y: (int)(worldPos.Y / CellSize)
        );
}
