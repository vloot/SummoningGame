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

    public void DisplayUI(bool isEnabled)
    {
        turnCanvas.enabled = isEnabled;
    }

    public void DisplayUI(bool isEnabled, Vector3 pos)
    {
        DisplayUI(isEnabled);
        parentRect.transform.position = pos; // TODO fix this
    }
}