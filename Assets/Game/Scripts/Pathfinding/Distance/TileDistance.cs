public class TileDistance : ITileDistance
{
    public int GetDistance(Tile tile1, Tile tile2)
    {
        var distX = UnityEngine.Mathf.Abs(tile1.position.x - tile2.position.x);
        var distY = UnityEngine.Mathf.Abs(tile1.position.y - tile2.position.y);

        if (distX > distY)
        {
            return distY + (distX - distY);
        }

        // distance in tiles
        return distX + (distY - distX);
    }
}