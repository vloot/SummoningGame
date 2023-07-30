using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public CharacterStats characterStats;
    public GameObject characterVisual;

    [SerializeField] private CharacterTurnController controller;

    // TODO character should track its position on grid

    private void OnMouseDown()
    {
        controller.CharacterClicked(this);
        ConsoleLogger.Log("Character clicked");
    }
}