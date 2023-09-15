public class MovementPathConfig : BasePathConfig
{
    public override bool IsTileAvailable(Tile tile)
    {
        return !tile.occupied;
    }
}
