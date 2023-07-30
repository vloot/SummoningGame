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

    [Header("Input")]
    [SerializeField] private InputController inputController;



    // current turn
    private bool _isCharacterSelected;
    private BaseCharacter _selectedCharacter;
    private CharacterAction _selectedCharacterAction;

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

        // initialize and set up commands
        // TODO setup commands
        var moveCommand = new MoveCommand(inputController, levelTiles);
        var attackCommand = new AttackCommand();
        var spellCommand = new SpellCommand();
        var itemCommand = new ItemCommand();

        _commandsDict = new Dictionary<CharacterAction, BaseCommand>();
        _commandsDict[CharacterAction.Move] = moveCommand;
        _commandsDict[CharacterAction.Attack] = attackCommand;
        _commandsDict[CharacterAction.UseSpell] = spellCommand;
        _commandsDict[CharacterAction.UseItem] = itemCommand;

        // Input controller needs tiles
        inputController.Setup(levelTiles);
    }


    public void CharacterClicked(BaseCharacter character)
    {
        if (!_isCharacterSelected)
        {
            // new character clicked
            SetupClickedCharacter(character);
            ConsoleLogger.Log("Character selected in controller");
        }

        // cases when character is selected
        if (character == _selectedCharacter)
        {
            // clicked on self
            return;
        }
        else
        {
            // other character was clicked
        }
    }

    private void SetupClickedCharacter(BaseCharacter character)
    {
        _selectedCharacter = character;
        inputController.acceptInputs = true;
        turnUI.DisplayUI(true, _selectedCharacter.transform.position);
    }

    private void TileClicked(Vector3Int tilePos)
    {
        if (_selectedCharacterAction == CharacterAction.Move)
        {
            _commandsDict[_selectedCharacterAction].ExecuteCommand(_selectedCharacter);
            CancelTurn(); // TODO end turn?
        }
    }

    private void EndTurn()
    {

    }

    private void CancelTurn()
    {
        turnUI.DisplayUI(false);
        inputController.acceptInputs = false;
        _selectedCharacter = null;
        _selectedCharacterAction = CharacterAction.None;
    }

    private void ActionSelected(CharacterAction action)
    {
        _selectedCharacterAction = action;
        ConsoleLogger.Log("Action selected: " + action);
        _commandsDict[action].PrepareCommand(_selectedCharacter);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelTurn(); // TODO looks questionable, fix or remove this
        }
    }
}
