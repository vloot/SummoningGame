using UnityEngine;
using System.Collections.Generic;

public class RangeHighlighter
{
    private LevelTiles _levelTiles;

    private ObjectPool<HightlightObject> _hightlightPool;
    private Pathfinder _pathfinder; // TODO this looks like it does not belong here

    private List<HightlightObject> _spawnedObjects;

    public RangeHighlighter(LevelTiles levelTiles, GameObject highlight, Transform highlightParent)
    {
        _hightlightPool = new ObjectPool<HightlightObject>(24, highlight, highlightParent);
        _levelTiles = levelTiles;
        _pathfinder = new Pathfinder(levelTiles);
        _spawnedObjects = new List<HightlightObject>();
    }

    public void Highlight(Tile originTile, byte range)
    {
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                var pos = new Vector3Int(x + originTile.position.x, y + originTile.position.y, 0);

                if (!_levelTiles.HasTile(pos) || originTile.position == pos)
                {
                    continue;
                }

                var tile = _levelTiles.tilesDict[pos];
                var dist = GetDistance(originTile, tile);

                if (dist == 0 || dist > range)
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

    public int GetDistance(Tile tile1, Tile tile2)
    {
        var path = _pathfinder.FindPath(tile1, tile2);
        return path.Count;
    }
}
