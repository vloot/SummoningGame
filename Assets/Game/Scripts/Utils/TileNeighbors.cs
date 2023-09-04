using UnityEngine;

public class TileNeighbors
{
    public static Vector3Int TopLeft = new(1, 1, 0);
    public static Vector3Int Top = new(1, 0, 0);
    public static Vector3Int TopRight = new(1, -1, 0);
    public static Vector3Int Left = new(0, 1, 0);
    public static Vector3Int Right = new(0, -1, 0);
    public static Vector3Int BottomLeft = new(-1, 1, 0);
    public static Vector3Int Bottom = new(-1, 0, 0);
    public static Vector3Int BottomRight = new(-1, -1, 0);
}
