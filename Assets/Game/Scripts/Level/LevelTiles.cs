using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class LevelTiles : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    public Dictionary<Vector3Int, Tile> tilesDict;

    private void Awake()
    {
        tilesDict = new Dictionary<Vector3Int, Tile>();

        // Get all tiles and store them in list as custom Tile objects
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            var tile = tilemap.GetTile(position);
            if (tile != null)
            {
                var customTile = new Tile(position, CellToWorld(position), tile);
                tilesDict[position] = customTile;
            }
        }
    }

    public bool HasTile(Vector3Int tilePosition)
    {
        return tilesDict.ContainsKey(tilePosition);
    }

    public Tile GetTile(Vector3Int tilePositions)
    {
        if (tilesDict.ContainsKey(tilePositions))
        {
            return tilesDict[tilePositions];
        }

        return null;
    }

    public Tile GetTileByWorldPosition(Vector3 tileWorldPosition)
    {
        tileWorldPosition.z = 0;
        var tilePos = tilemap.WorldToCell(tileWorldPosition);
        return GetTile(tilePos);
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return tilemap.WorldToCell(worldPosition);
    }

    public Vector3 CellToWorld(Vector3Int position)
    {
        return tilemap.CellToWorld(position);
    }
}
