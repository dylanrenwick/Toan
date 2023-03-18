using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

using Toan.ECS;
using Toan.ECS.Resources;

namespace Toan.Physics;

public class SpatialMap : Resource
{
    private readonly Dictionary<Point, HashSet<Guid>> _spatialTable = new();
    private readonly Dictionary<Guid, HashSet<Point>> _lookupTable  = new();

    public required int CellSize { get; init; }

    /// <summary>
    /// Fetches a <see cref="IReadOnlySet{}"/> of entity IDs found at the given cell coordinate
    /// </summary>
    /// <param name="x">X position in cell coordinates</param>
    /// <param name="y">Y position in cell coordinates</param>
    /// <returns></returns>
    public IReadOnlySet<Guid> this[int x, int y]
    {
        get
        {
            Point cellPos = new(x, y);
            return _spatialTable.TryGetValue(cellPos, out var cell)
                ? cell
                : new HashSet<Guid>();
        }
    }

    public IEnumerable<Point> OccupiedCells
    => _spatialTable
        .Where(kvp => kvp.Value.Count > 0)
        .Select(kvp => kvp.Key);

    public bool Add(Entity entity)
        => Add(entity, entity.Get<Collider>());

    public bool Add(Entity entity, Collider collider)
        => Add(entity.Id, CollisionHelper.GetColliderBoundingBox(entity, collider));

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
        if (!_lookupTable.ContainsKey(entityId))
            return;

        foreach (var cell in _lookupTable[entityId])
        {
            RemoveFromCell(entityId, cell);
        }
    }

    private void RemoveFromCell(Guid entityId, Point cell)
    {
        if (_lookupTable.ContainsKey(entityId))
        {
            RemoveLookup(entityId, cell);
        }

        if (_spatialTable.ContainsKey(cell))
        {
            RemoveSpatial(entityId, cell);
        }
    }

    private void RemoveSpatial(Guid entityId, Point cell)
    {
        HashSet<Guid> cellContents = _spatialTable[cell];
        if (cellContents.Remove(entityId) && cellContents.Count == 0)
        {
            _spatialTable.Remove(cell);
        }
    }

    private void RemoveLookup(Guid entityId, Point cell)
    {
        HashSet<Point> lookupContents = _lookupTable[entityId];
        if (lookupContents.Remove(cell) && lookupContents.Count == 0)
        {
            _lookupTable.Remove(entityId);
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
        {
            _spatialTable.Add(cell, new HashSet<Guid>());
        }

        if (!_lookupTable.ContainsKey(guid))
        {
            _lookupTable.Add(guid, new HashSet<Point>());
        }

        return _spatialTable[cell].Add(guid)
            && _lookupTable[guid].Add(cell);
    }

    private static IEnumerable<Point> GetCellsInBounds(Rectangle boundingBox)
    {
        for (int y = boundingBox.Y; y < boundingBox.Y + boundingBox.Height; y++)
        {
            for (int x = boundingBox.X; x < boundingBox.X + boundingBox.Width; x++)
            {
                yield return new(x, y);
            }
        }
    }

    private Rectangle CellBoundsFromRealBounds(FloatRect boundingBox)
    {
        float cellOffsetX = boundingBox.Left % CellSize;
        float cellOffsetY = boundingBox.Top % CellSize;
        if (cellOffsetX < 0)
            cellOffsetX += CellSize;
        if (cellOffsetY < 0)
            cellOffsetY += CellSize;

        return new(
            x      : (int)Math.Floor(boundingBox.Left / CellSize),
            y      : (int)Math.Floor(boundingBox.Top / CellSize),
            width  : (int)Math.Ceiling((cellOffsetX + boundingBox.Width) / CellSize),
            height : (int)Math.Ceiling((cellOffsetY + boundingBox.Height) / CellSize)
        );
    }
}
