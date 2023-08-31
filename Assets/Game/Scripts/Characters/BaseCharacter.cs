using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public CharacterStats characterStats;
    public GameObject characterVisual;

    [SerializeField] private CharacterTurnController controller;

    // TODO character should track its position on grid (PROPERLY)
    public Tile characterTile;

    private void Start()
    {
        // FIXME remove this bit
        var lt = FindObjectOfType<LevelTiles>();
        characterTile = lt.GetTileByWorldPosition(transform.position);
    }

    private void OnMouseDown()
    {
        // controller.CharacterClicked(this);
        // ConsoleLogger.Log("Character clicked");
    }
}