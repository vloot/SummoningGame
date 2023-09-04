using UnityEngine;

public class AttackPathfindingConfig : BasePathfindingConfig
{
    public override bool IsTileAvailable(Vector3Int position)
    {
        return true;
    }
}
