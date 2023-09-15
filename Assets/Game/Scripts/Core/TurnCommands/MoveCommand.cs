using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class MoveCommand : BaseCommand
{

    private BasePathConfig _pathConfig;

    public MoveCommand(InputController inputController, LevelTiles levelTiles, RangeHighlighter rangeHighlighter, Pathfinder pathfinder)
    {
        InitBaseParamaters(inputController, levelTiles, rangeHighlighter, pathfinder);
        _actionTarget = ActionTarget.Tile;
        _pathConfig = new MovementPathConfig();
    }

    public override bool ExecuteCommand(BaseCharacter character, System.Action onComplete = null)
    {
        var path = _pathfinder.FindPath(_levelTiles.GetTileByWorldPosition(character.transform.position), _inputController.ClickedTile, config: _pathConfig);

        if (path.Count > character.characterStats.moveRange || path.Count == 0)
        {
            ConsoleLogger.Log("Out of movement range");
            return false;
        }

        _rangeHighlighter.RemoveHighlight();

        // update new tile position for the character
        character.tile.occupied = false;
        character.tile = path[path.Count - 1];
        character.tile.occupied = true;

        var pathArray = CreateWorldPath(path);
        var tween = character.gameObject.transform.DOPath(pathArray, pathArray.Length * 0.5f); // TODO move this to character animator, remove hardcoded values
        tween.onComplete += () => onComplete?.Invoke(); // FIXME not a good idea to have game logic run AFTER animation

        return true;
    }

    public override void PrepareCommand(BaseCharacter character)
    {
        // highlight range
        _rangeHighlighter.RemoveHighlight();
        _rangeHighlighter.Highlight(character.tile, character.characterStats.moveRange);
    }

    private Vector3[] CreateWorldPath(List<Tile> path)
    {
        var pathArray = new Vector3[path.Count];

        for (int i = 0; i < pathArray.Length; i++)
        {
            var pos = _levelTiles.CellToWorld(path[i].position);
            pos.y += 0.25f; // FIXME remove hardcoded offset
            pathArray[i] = pos;
        }

        return pathArray;
    }
}
