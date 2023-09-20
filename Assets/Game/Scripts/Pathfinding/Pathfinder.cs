using UnityEngine;
using System.Collections.Generic;

public class Pathfinder
{
    private LevelTiles _levelTiles;
    private BasePathConfig _defaultConfig;
    private ITileDistance _defaultDistance;
    private Vector3Int[] _offsets;

    public Pathfinder(LevelTiles levelTiles)
    {
        _levelTiles = levelTiles;
        _defaultConfig = new BasePathConfig();
        _defaultDistance = new ManhattanTileDistance();
        _offsets = new Vector3Int[] {
            TileNeighbors.Top, TileNeighbors.Right, TileNeighbors.Bottom, TileNeighbors.Left
        };
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

    public List<Tile> FindPath(Tile startTile, Tile targetTile, ITileDistance distance = null, BasePathConfig config = null)
    {
        distance ??= _defaultDistance;
        config ??= _defaultConfig;

        var open = new Heap<Tile>(_levelTiles.GetTilesCount());
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

            foreach (var n in GetNeighbours(currentTile, config))
            {
                if (closed.Contains(n))
                {
                    continue;
                }

                int newCost = currentTile.gCost + distance.GetDistance(currentTile, n);
                if (newCost < n.gCost || !open.Contains(n))
                {
                    n.gCost = newCost;
                    n.hCost = distance.GetDistance(n, targetTile);
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

    public List<Tile> GetNeighbours(Tile tile, BasePathConfig config, int depth = 1)
    {
        var neighbours = new List<Tile>();

        for (int depthMultiplier = 1; depthMultiplier <= depth; depthMultiplier++)
        {
            AddNeighboursToList(neighbours, tile, config, depthMultiplier);
        }

        return neighbours;
    }

    private void AddNeighboursToList(List<Tile> neighbours, Tile tile, BasePathConfig config, int depthMultiplier)
    {
        for (int offsetIndex = 0; offsetIndex < _offsets.Length; offsetIndex++)
        {
            var position = tile.position + _offsets[offsetIndex] * depthMultiplier;

            if (_levelTiles.HasTile(position) && config.IsTileAvailable(_levelTiles.GetTile(position)))
            {
                neighbours.Add(_levelTiles.GetTile(position));
            }
        }
    }
}