public class AIAttack
{
    private BaseCommand _command;
    private ITileDistance _tileDistance;
    private InputController _inputController;

    public AIAttack(BaseCommand command, InputController inputController)
    {
        _command = command;
        _tileDistance = new ManhattanTileDistance();
        _inputController = inputController;
    }

    public bool TryPerformAction(BaseCharacter character, BaseCharacter targetCharacter, System.Action onComplete = null)
    {
        if (_tileDistance.GetDistance(character.tile, targetCharacter.tile) <= character.characterStats.attackRange)
        {
            AttackCharacter(character, targetCharacter);
            return true;
        }

        return false;
    }

    private void AttackCharacter(BaseCharacter character, BaseCharacter targetCharacter)
    {
        _inputController.SimulateClick(targetCharacter);
        _command.ExecuteCommand(character);
    }
}
