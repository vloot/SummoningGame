public class ManhattanTileDistance : ITileDistance
{
    public int GetDistance(Tile tile1, Tile tile2)
    {
        return UnityEngine.Mathf.Abs(tile1.position.x - tile2.position.x) + UnityEngine.Mathf.Abs(tile1.position.y - tile2.position.y);
    }
}