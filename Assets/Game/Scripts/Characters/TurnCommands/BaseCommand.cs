using UnityEngine;

public abstract class BaseCommand
{
    private InputController _inputController;

    public abstract void PrepareCommand(BaseCharacter character);
    public abstract void ExecuteCommand(BaseCharacter character);
}
