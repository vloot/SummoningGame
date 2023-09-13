using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterTurnUI : MonoBehaviour
{
    [SerializeField] private Canvas turnCanvas;

    [Header("UI")]
    [SerializeField] private RectTransform parentRect;

    [Header("Action Buttons")]
    [SerializeField] private Button moveButton;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button spellButton;

    [Header("Other Buttons")]
    [SerializeField] private Button endTurnButton;

    public void SetupTurnUI(UnityAction moveEvent, UnityAction attackEvent, UnityAction spellEvent, UnityAction endTurnEvent)
    {
        moveButton.onClick.AddListener(moveEvent);
        attackButton.onClick.AddListener(attackEvent);
        spellButton.onClick.AddListener(spellEvent);
        endTurnButton.onClick.AddListener(endTurnEvent);
    }

    public void ShowUI(CharacterTurn characterTurn)
    {
        ShowUI(!characterTurn.IsOver());
        if (characterTurn.IsOver()) return;

        SetUIToCharacterPosition(characterTurn.character);
        UpdateUI(characterTurn);
    }

    public void ShowUI(bool isEnabled)
    {
        turnCanvas.enabled = isEnabled;
    }

    public void UpdateUI(CharacterTurn characterTurn)
    {
        // TODO instead of calling this directly, use an event to call it
        // TODO Simplify this, and use CharacterAction, instead of an entire CharacterTurn object
        moveButton.interactable = !characterTurn.Moved;
        attackButton.interactable = !characterTurn.Attacked;
        spellButton.interactable = !characterTurn.CastedSpell;
    }

    private void SetUIToCharacterPosition(BaseCharacter character)
    {
        parentRect.transform.position = character.transform.position; // TODO Do this properly
    }
}