using UnityEngine;
using System.Collections.Generic;

public class Pathfinder
{
    private LevelTiles _levelTiles;

    public Pathfinder(LevelTiles levelTiles)
    {
        _levelTiles = levelTiles;
    }

    public List<Tile> ReconstructPath(Tile startTile, Tile targetTile)
    {
        var path = new List<Tile>();
        var currentTile = targetTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parentTile;
        }

        path.Reverse();
        return path;
    }

    public List<Tile> FindPath(Tile startTile, Tile targetTile)
    {
        var open = new Heap<Tile>(_levelTiles.tilesDict.Count);
        var closed = new HashSet<Tile>();

        open.Add(startTile);

        while (open.Count > 0)
        {
            var currentTile = open.RemoveFirst();
            closed.Add(currentTile);

            if (currentTile == targetTile)
            {
                // done
                return ReconstructPath(startTile, targetTile);
            }

            foreach (var n in GetNeighbours(currentTile))
            {
                if (closed.Contains(n))
                {
                    continue;
                }

                int newCost = currentTile.gCost + GetDistance(currentTile, n);
                if (newCost < n.gCost || !open.Contains(n))
                {
                    n.gCost = newCost;
                    n.hCost = GetDistance(n, targetTile);
                    n.parentTile = currentTile;

                    if (!open.Contains(n))
                    {
                        open.Add(n);
                    }
                }


            }
        }

        // no path found
        return new List<Tile>();
    }

    private int GetDistance(Tile tile1, Tile tile2)
    {
        return Mathf.Abs(tile1.position.x - tile2.position.x) + Mathf.Abs(tile1.position.y - tile2.position.y);
    }

    private List<Tile> GetNeighbours(Tile tile)
    {
        var offsets = new Vector3Int[] {
            // top left
            // new Vector3Int(1, 1, 0),

            // top
            new Vector3Int(1, 0, 0),

            // top right
            // new Vector3Int(1, -1, 0),
            
            // left
            new Vector3Int(0, 1, 0),

            // right
            new Vector3Int(0, -1, 0),

            // bottom left
            // new Vector3Int(-1, 1, 0),

            // bottom
            new Vector3Int(-1, 0, 0),

            // bottom right
            // new Vector3Int(-1, -1, 0),
        };

        var neighbours = new List<Tile>();

        foreach (var offset in offsets)
        {
            var position = tile.position + offset;
            if (_levelTiles.tilesDict.ContainsKey(position) && !_levelTiles.tilesDict[position].occupied)
            {
                neighbours.Add(_levelTiles.tilesDict[position]);
            }
        }

        return neighbours;
    }
}
