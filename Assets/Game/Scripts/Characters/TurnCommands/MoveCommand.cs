using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class MoveCommand : BaseCommand
{
    private Pathfinder _pathfinder;
    private LevelTiles _levelTiles;

    private CharacterTurnController _turnController;
    private InputController _inputController;

    public MoveCommand(InputController inputController, LevelTiles levelTiles)
    {
        _pathfinder = new Pathfinder(levelTiles);
        _inputController = inputController;
        _levelTiles = levelTiles;
    }

    public override void ExecuteCommand(BaseCharacter character)
    {
        var path = _pathfinder.FindPath(_levelTiles.GetTileByWorldPosition(character.transform.position), _levelTiles.GetTile(_inputController.ClickPosition));

        var pathList = new List<Vector3>();

        foreach (var item in path)
        {
            var pos = _levelTiles.CellToWorld(item.tilePosition);
            pos.y += 0.25f;
            pathList.Add(pos);
        }

        character.gameObject.transform.DOPath(pathList.ToArray(), pathList.Count * 0.5f);
    }

    public override void PrepareCommand(BaseCharacter character)
    {
        // highlight range
        // character.characterStats.moveRange
    }
}
