using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterTurnController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Pathfinder pathfinder;
    [SerializeField] private LevelTiles levelTiles;

    [Header("UI")]
    [SerializeField] private CharacterTurnUI turnUI;

    [Header("Tile highlighting")] // TODO this shouldn't be here
    [SerializeField] private GameObject highlightPrefab;
    [SerializeField] private Transform highlightPoolParent;
    private RangeHighlighter _rangeHighlighter;

    [Header("Input")]
    [SerializeField] private InputController inputController;

    private CharacterTurn _characterTurn;
    private Dictionary<CharacterAction, BaseCommand> _commandsDict;


    private void Awake()
    {
        inputController.OnTileClicked += TileClicked;
        inputController.OnCharacterClicked += CharacterClicked;

        turnUI.SetupTurnUI(
            () => ActionSelected(CharacterAction.Move),
            () => ActionSelected(CharacterAction.Attack),
            () => ActionSelected(CharacterAction.UseSpell)
        );

        // TODO should this be here?
        _rangeHighlighter = new RangeHighlighter(levelTiles, highlightPrefab, highlightPoolParent);
        var pathfinder = new Pathfinder(levelTiles);

        // initialize and set up commands
        var moveCommand = new MoveCommand(inputController, levelTiles, _rangeHighlighter, pathfinder);
        var attackCommand = new AttackCommand(inputController, levelTiles, _rangeHighlighter, pathfinder);
        var spellCommand = new SpellCommand();
        var itemCommand = new ItemCommand();

        _commandsDict = new Dictionary<CharacterAction, BaseCommand>
        {
            [CharacterAction.Move] = moveCommand,
            [CharacterAction.Attack] = attackCommand,
            [CharacterAction.UseSpell] = spellCommand,
            [CharacterAction.UseItem] = itemCommand
        };

        // Input controller needs tiles
        inputController.Setup(levelTiles);
    }


    public void CharacterClicked(BaseCharacter character)
    {
        if (!_characterTurn.IsCharacterSelected() && character.CanMove())
        {
            // new turn
            SetupClickedCharacter(character);
            return;
        }

        // execute command if possible
        TryExecuteCommand(ActionTarget.Character);
    }

    private void SetupClickedCharacter(BaseCharacter character)
    {
        // new character clicked
        _characterTurn = new CharacterTurn(character);
        turnUI.ShowUI(_characterTurn);
        // inputController.acceptInputs = true;

        ConsoleLogger.Log("Character selected in controller");
    }

    private void TileClicked(Vector3Int tilePos)
    {
        TryExecuteCommand(ActionTarget.Tile);
    }

    private void TryExecuteCommand(ActionTarget actionTarget)
    {
        if (_characterTurn.characterAction == CharacterAction.None)
        {
            return;
        }

        if (_commandsDict[_characterTurn.characterAction].CanExecute(actionTarget))
        {
            inputController.acceptInputs = false;

            var wasExecuted = _commandsDict[_characterTurn.characterAction].ExecuteCommand(_characterTurn.character, () => CommandCompleted());

            if (wasExecuted)
            {
                turnUI.ShowUI(false);
            }
            else
            {
                inputController.acceptInputs = true;
            }
        }
    }

    private void CommandCompleted()
    {
        _characterTurn.CompleteAction();
        turnUI.ShowUI(_characterTurn);
        inputController.acceptInputs = true;

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

    private void ActionSelected(CharacterAction action)
    {
        _characterTurn.SelectAction(action);
        _commandsDict[action].PrepareCommand(_characterTurn.character);

        ConsoleLogger.Log("Action selected: " + action);
    }
}
