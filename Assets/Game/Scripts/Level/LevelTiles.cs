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

        // Get all tiles and store them in list as Tile objects
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            var tile = tilemap.GetTile(position);
            if (tile != null)
            {
                var customTile = new Tile(position, tilemap.GetTile(position));
                tilesDict[position] = customTile;
            }
        }
    }

    private void Start()
    {

    }

    public bool HasTile(Vector3Int tilePos)
    {
        return tilesDict.ContainsKey(tilePos);
    }

    public Tile GetTile(Vector3Int tilePos)
    {
        if (tilesDict.ContainsKey(tilePos))
        {
            return tilesDict[tilePos];
        }

        return null;
    }

    public Tile GetTileByWorldPosition(Vector3 tilePosWorld)
    {
        tilePosWorld.z = 0;
        var tilePos = tilemap.WorldToCell(tilePosWorld);
        return GetTile(tilePos);
    }

    public Vector3Int WorldToCell(Vector3 worldPos)
    {
        return tilemap.WorldToCell(worldPos);
    }

    public Vector3 CellToWorld(Vector3Int pos)
    {
        return tilemap.CellToWorld(pos);
    }
}
