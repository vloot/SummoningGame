using UnityEngine;
using System.Collections.Generic;

public class CharacterTurnController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ComponentInit componentInit;
    [SerializeField] private LevelTiles levelTiles;

    [Header("UI")]
    [SerializeField] private CharacterTurnUI turnUI;

    private RangeHighlighter _rangeHighlighter;

    [Header("Input")]
    [SerializeField] private InputController inputController;

    private CharacterTurn _characterTurn;
    private Dictionary<CharacterAction, BaseCommand> _commandsDict;


    private void Awake()
    {
        inputController.Setup(levelTiles);
        inputController.OnTileClicked += TileClicked;
        inputController.OnCharacterClicked += CharacterClicked;

        turnUI.SetupTurnUI(
            () => CommandSelectedOnUI(CharacterAction.Move),
            () => CommandSelectedOnUI(CharacterAction.Attack),
            () => CommandSelectedOnUI(CharacterAction.UseSpell),
            () => EndTurn()
        );
    }

    private void Start()
    {
        _commandsDict = componentInit.CommandsDict;
        _rangeHighlighter = componentInit.RangeHighlighter;
    }

    private void CommandSelectedOnUI(CharacterAction action)
    {
        _characterTurn.SelectAction(action);
        _commandsDict[action].PrepareCommand(_characterTurn.character);

        ConsoleLogger.Log("Action selected: " + action);
    }

    public void CharacterClicked(BaseCharacter character)
    {
        if (!_characterTurn.IsCharacterSelected() && character.team.ControlledByPlayer && character.CanMove())
        {
            // new turn, new character clicked
            _characterTurn = new CharacterTurn(character);
            turnUI.ShowUI(_characterTurn);
            // inputController.acceptInputs = true;

            ConsoleLogger.Log("Character selected in controller");
            return;
        }

        // execute command if possible
        TryExecuteCommand(ActionTarget.Character);
    }

    private void TileClicked(Tile tile)
    {
        TryExecuteCommand(ActionTarget.Tile);
    }

    private void TryExecuteCommand(ActionTarget actionTarget)
    {
        if (_characterTurn.characterAction == CharacterAction.None || _characterTurn.IsOver())
        {
            return;
        }

        if (_commandsDict[_characterTurn.characterAction].CanExecute(actionTarget))
        {
            inputController.acceptPlayerInputs = false;
            turnUI.ShowUI(false);

            var wasExecuted = _commandsDict[_characterTurn.characterAction].ExecuteCommand(_characterTurn.character, () => CommandCompleted());

            if (!wasExecuted)
            {
                turnUI.ShowUI(true);
                inputController.acceptPlayerInputs = true;
            }
        }
    }

    private void CommandCompleted()
    {
        _characterTurn.CompleteAction();
        turnUI.ShowUI(_characterTurn);
        inputController.acceptPlayerInputs = true;

        if (_characterTurn.IsOver())
        {
            EndTurn();
        }
    }

    private void EndTurn()
    {
        ConsoleLogger.Log("Turn ended");
        turnUI.ShowUI(false);
        _rangeHighlighter.RemoveHighlight();
        // inputController.acceptInputs = false;
        _characterTurn.EndTurn();
    }
}