using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterTurnUI : MonoBehaviour
{
    [SerializeField] private Canvas turnCanvas;

    [Header("UI")]
    [SerializeField] private RectTransform parentRect;

    [Header("Buttons")]
    [SerializeField] private Button moveButton;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button spellButton;

    public void SetupTurnUI(UnityAction moveEvent, UnityAction attackEvent, UnityAction spellEvent)
    {
        moveButton.onClick.AddListener(moveEvent);
        attackButton.onClick.AddListener(attackEvent);
        spellButton.onClick.AddListener(spellEvent);
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
        moveButton.interactable = !characterTurn.Moved;
        attackButton.interactable = !characterTurn.Attacked;
        spellButton.interactable = !characterTurn.CastedSpell;
    }

    private void SetUIToCharacterPosition(BaseCharacter character)
    {
        parentRect.transform.position = character.transform.position; // TODO fix this
    }
}