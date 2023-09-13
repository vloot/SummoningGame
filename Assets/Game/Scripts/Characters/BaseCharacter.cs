using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public CharacterStats characterStats;
    public CharacterVitals characterVitals;

    // Character should track its position on grid
    public Tile tile;

    // Teams
    public Team team;

    // character events
    public delegate void OnTurnEnded(BaseCharacter character);
    public OnTurnEnded OnCharacterTurnEnded;

    public void CreateCharacter(Tile tile, Team team)
    {
        this.tile = tile;
        this.team = team;

        tile.occupied = true;

        // FIXME this is wrong
        var pos = tile.worldPosition;
        pos.y += 0.25f; // FIXME remove hardcoded offset
        transform.position = pos;
    }

    private void Start()
    {
        // FIXME redo this PROPERLY
        var lt = FindObjectOfType<LevelTiles>();
        tile = lt.GetTileByWorldPosition(transform.position);
    }

    public bool CanMove()
    {
        return team.CanMove(this);
    }

    public void EndTurn()
    {
        OnCharacterTurnEnded?.Invoke(this);
    }

    public byte CalculateDamage()
    {
        return characterStats.strength;
    }

    public void TakeDamage(byte amount)
    {
        characterVitals.health -= amount;
    }
}