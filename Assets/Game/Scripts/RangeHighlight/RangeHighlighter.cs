using UnityEngine;
using System.Collections.Generic;

public class RangeHighlighter
{
    private LevelTiles _levelTiles;

    private ObjectPool<HightlightObject> _hightlightPool;
    private Pathfinder _pathfinder; // TODO this looks like it does not belong here

    private List<HightlightObject> _spawnedObjects;

    private ITileDistance _distanceCalculator;

    public RangeHighlighter(LevelTiles levelTiles, GameObject highlight, Transform highlightParent)
    {
        _hightlightPool = new ObjectPool<HightlightObject>(24, highlight, highlightParent);
        _levelTiles = levelTiles;
        _pathfinder = new Pathfinder(levelTiles);
        _spawnedObjects = new List<HightlightObject>();
    }

    public void Highlight(Tile originTile, byte maxRange, byte minRange = 0, bool walkableOnly = true)
    {
        for (int x = -maxRange; x <= maxRange; x++)
        {
            for (int y = -maxRange; y <= maxRange; y++)
            {
                var pos = new Vector3Int(x + originTile.position.x, y + originTile.position.y, 0);

                if (!_levelTiles.HasTile(pos) || originTile.position == pos)
                {
                    continue;
                }

                var tile = _levelTiles.tilesDict[pos];
                var dist = walkableOnly ? GetWalkableDistance(originTile, tile) : GetTileDistnace(originTile, tile);

                if (dist == 0 || dist > maxRange || dist < minRange)
                {
                    continue;
                }

                _spawnedObjects.Add(_hightlightPool.Spawn(_levelTiles.CellToWorld(pos)));
            }
        }
    }

    public void RemoveHighlight()
    {
        ConsoleLogger.Log("Removing highlight");
        foreach (var item in _spawnedObjects)
        {
            _hightlightPool.Despawn(item);
        }

        _spawnedObjects.Clear();
    }

    public int GetWalkableDistance(Tile tile1, Tile tile2)
    {
        var path = _pathfinder.FindPath(tile1, tile2);
        return path.Count;
    }

    private int GetTileDistnace(Tile tile1, Tile tile2)
    {
        return _distanceCalculator.GetDistance(tile1, tile2);
    }
}
