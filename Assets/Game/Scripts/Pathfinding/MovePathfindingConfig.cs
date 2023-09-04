using UnityEngine;

public class MovePathfindingConfig : BasePathfindingConfig
{
    private LevelTiles _levelTiles;

    public MovePathfindingConfig(LevelTiles levelTiles)
    {
        _levelTiles = levelTiles;
    }

    public override bool IsTileAvailable(Vector3Int position)
    {
        return !_levelTiles.tilesDict[position].occupied;
    }
}
