public abstract class BaseCommand
{
    protected InputController _inputController;
    protected RangeHighlighter _rangeHighlighter;
    protected Pathfinder _pathfinder;
    protected LevelTiles _levelTiles;

    protected ActionTarget _actionTarget;

    public abstract void PrepareCommand(BaseCharacter character);

    /// <summary>
    /// Returns true if the command executed succesfully, false otherwise
    /// </summary>
    /// <param name="character">Character reference</param>
    /// <param name="onComplete">(optional) action to be executed once the command is succesfully executed</param>
    public abstract bool ExecuteCommand(BaseCharacter character, System.Action onComplete = null);

    public virtual bool CanExecute(ActionTarget actionTarget)
    {
        return actionTarget == _actionTarget;
    }

    protected void InitBaseParamaters(InputController inputController, LevelTiles levelTiles, RangeHighlighter rangeHighlighter, Pathfinder pathfinder)
    {
        _pathfinder = pathfinder;
        _inputController = inputController;
        _levelTiles = levelTiles;
        _rangeHighlighter = rangeHighlighter;
    }
}
