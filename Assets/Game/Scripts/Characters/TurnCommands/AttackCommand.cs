public class AttackCommand : BaseCommand
{

    public AttackCommand(InputController inputController, LevelTiles levelTiles, RangeHighlighter rangeHighlighter, Pathfinder pathfinder)
    {
        InitBaseParamaters(inputController, levelTiles, rangeHighlighter, pathfinder);
        _actionTarget = ActionTarget.Character;
    }

    public override bool ExecuteCommand(BaseCharacter character, System.Action onComplete = null)
    {
        if (_inputController.Character == character)
        {
            // clicked on self
            return false;
        }

        var pathLength = _pathfinder.FindPath(character.characterTile, _inputController.Character.characterTile).Count;

        if (pathLength == 0 || pathLength > character.characterStats.attackRange)
        {
            return false;
        }

        _rangeHighlighter.RemoveHighlight();

        ConsoleLogger.Log("Attacking " + _inputController.Character.gameObject.name);
        _inputController.Character.characterStats.health -= character.characterStats.strength;

        onComplete?.Invoke();

        return true;
    }

    public override void PrepareCommand(BaseCharacter character)
    {
        _rangeHighlighter.Highlight(character.characterTile, character.characterStats.attackRange);
    }
}