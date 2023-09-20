public class AIMove
{
    private BaseCommand _command;
    private Pathfinder _pathfinder;
    private InputController _inputController;
    private BasePathConfig _movementConfig;

    public AIMove(BaseCommand command, Pathfinder pathfinder, InputController inputController)
    {
        _command = command;
        _pathfinder = pathfinder;
        _inputController = inputController;
        _movementConfig = new MovementPathConfig();
    }

    public bool TryPerformAction(BaseCharacter character, BaseCharacter targetCharacter, System.Action onComplete = null, int depth = 1)
    {
        var characterMove = CalculateMoveToCharacter(character, targetCharacter, _movementConfig, depth);
        if (characterMove.canMove)
        {
            MoveToTile(character, characterMove.tile, onComplete);
        }
        return characterMove.canMove;
    }

    private AICharacterMove CalculateMoveToCharacter(BaseCharacter character, BaseCharacter enemyCharacter, BasePathConfig pathConfig, int depth)
    {
        var neighborTiles = _pathfinder.GetNeighbours(enemyCharacter.tile, pathConfig, depth);
        if (neighborTiles.Count == 0)
        {
            return new AICharacterMove();
        }

        var minDistance = character.characterStats.moveRange + 1;
        var targetTile = neighborTiles[0];

        foreach (var tile in neighborTiles)
        {
            var pathLength = _pathfinder.FindPath(character.tile, tile).Count;
            if (pathLength != 0 && pathLength < minDistance)
            {
                minDistance = pathLength;
                targetTile = tile;
            }
        }

        if (targetTile != null && minDistance <= character.characterStats.moveRange)
        {
            // Can move
            return new AICharacterMove(enemyCharacter, targetTile);
        }

        return new AICharacterMove();
    }

    private void MoveToTile(BaseCharacter character, Tile targetTile, System.Action onComplete)
    {
        _inputController.SimulateClick(targetTile);
        _command.ExecuteCommand(character, onComplete);
    }
}
