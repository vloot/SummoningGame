public class AttackCommand : BaseCommand
{
    private BasePathfindingConfig _pathfindingConfig;

    public AttackCommand(InputController inputController, LevelTiles levelTiles, RangeHighlighter rangeHighlighter, Pathfinder pathfinder)
    {
        InitBaseParamaters(inputController, levelTiles, rangeHighlighter, pathfinder);
        _actionTarget = ActionTarget.Character;
        _pathfindingConfig = new AttackPathfindingConfig();
    }

    public override bool ExecuteCommand(BaseCharacter character, System.Action onComplete = null)
    {
        if (_inputController.ClickedCharacter == character || _inputController.ClickedCharacter.team.Side == character.team.Side)
        {
            // clicked on self, or teammate
            ConsoleLogger.Log("clicked on self, or teammate");
            return false;
        }

        var pathLength = _pathfinder.FindPath(character.tile, _inputController.ClickedCharacter.tile, _pathfindingConfig).Count;

        if (pathLength == 0 || pathLength > character.characterStats.attackRange)
        {
            return false;
        }

        _rangeHighlighter.RemoveHighlight();

        ConsoleLogger.Log("Attacking " + _inputController.ClickedCharacter.gameObject.name);
        _inputController.ClickedCharacter.characterVitals.health -= character.characterStats.strength;

        onComplete?.Invoke();

        return true;
    }

    public override void PrepareCommand(BaseCharacter character)
    {
        _rangeHighlighter.RemoveHighlight();
        _rangeHighlighter.Highlight(character.tile, character.characterStats.attackRange, _pathfindingConfig);
    }
}