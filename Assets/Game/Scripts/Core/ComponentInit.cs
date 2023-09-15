using UnityEngine;
using System.Collections.Generic;

public class ComponentInit : MonoBehaviour
{
    [SerializeField] public InputController inputController;
    [SerializeField] public LevelTiles levelTiles;
    [SerializeField] public Pathfinder pathfinder;

    [Header("Range highlight")]
    [SerializeField] private GameObject highlightPrefab;
    [SerializeField] private Transform highlightPoolParent;

    public RangeHighlighter RangeHighlighter { get; private set; }
    public Dictionary<CharacterAction, BaseCommand> CommandsDict { get; private set; }

    private void Awake()
    {
        RangeHighlighter = new RangeHighlighter(levelTiles, highlightPrefab, highlightPoolParent);
        pathfinder = new Pathfinder(levelTiles);

        CommandsDict = new Dictionary<CharacterAction, BaseCommand>
        {
            [CharacterAction.Move] = new MoveCommand(inputController, levelTiles, RangeHighlighter, pathfinder),
            [CharacterAction.Attack] = new AttackCommand(inputController, levelTiles, RangeHighlighter, pathfinder),
            [CharacterAction.UseSpell] = new SpellCommand(),
            [CharacterAction.UseItem] = new ItemCommand()
        };
    }
}
