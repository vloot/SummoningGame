using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Tile : IHeapItem<Tile>
{
    public Vector3Int position;
    public Vector3 worldPosition;
    public TileBase tileBase;

    public bool occupied;

    // pathfinding
    public int gCost;
    public int hCost;
    public int fCost { get => gCost + hCost; }
    public int heapIndex;

    public int HeapIndex { get => heapIndex; set => heapIndex = value; }

    public Tile parentTile;

    public Tile(Vector3Int position, Vector3 worldPosition, TileBase tileRef)
    {
        this.position = position;
        this.worldPosition = worldPosition;

        tileBase = tileRef;
    }

    public void ClearTile()
    {
        // todo clear pathfinding data
    }

    public int CompareTo(Tile other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
}
