using UnityEngine;

public class RangeHighlight : MonoBehaviour
{

    public GameObject highlight;

    private ObjectPool<HightlightObject> _hightlightPool;

    private void Awake()
    {
        _hightlightPool = new ObjectPool<HightlightObject>(24, highlight, transform);
    }

    private void Start()
    {
        var lt = FindObjectOfType<LevelTiles>();
        // -1 -2 0
        Highlight(lt.GetTile(new Vector3Int(0, 0, 0)), 3);
    }

    public void Highlight(Tile originTile, byte range)
    {
        var lt = FindObjectOfType<LevelTiles>();

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                var pos = new Vector3Int(x + originTile.tilePosition.x, y + originTile.tilePosition.y, 0);

                if (!lt.HasTile(pos) || originTile.tilePosition == pos)
                {
                    continue;
                }

                var tile = lt.tilesDict[pos];

                if (GetDistance(originTile, tile) > range)
                {
                    print("range check at " + pos);
                    continue;
                }

                _hightlightPool.Spawn(lt.CellToWorld(pos));
                print(pos);
            }
        }
    }

    public static int GetDistance(Tile tile1, Tile tile2)
    {
        var lt = FindObjectOfType<LevelTiles>();
        var pf = new Pathfinder(lt);

        var path = pf.FindPath(tile1, tile2);

        return path.Count;
    }
}
