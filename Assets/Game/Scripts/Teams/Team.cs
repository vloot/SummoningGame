using System.Collections.Generic;
using System.Linq;

public class Team
{
    private List<BaseCharacter> characters;
    TurnData<BaseCharacter> characterTurnData;

    public TeamSide Side { get; private set; }
    public bool ControlledByPlayer { get; private set; }

    public bool isActiveTurn;

    // events
    public delegate void OnTurnEndedDelegate(Team t);
    public OnTurnEndedDelegate OnTurnEnded;

    public Team(TeamSide side, bool controlledByPlayer)
    {
        Side = side;
        ControlledByPlayer = controlledByPlayer;

        characters = new List<BaseCharacter>();
        // true if charactewr can move, false otherwise
        characterTurnData = new TurnData<BaseCharacter>();
    }

    public void AddCharacter(BaseCharacter character)
    {
        characters.Add(character);
        characterTurnData.AddElement(character);
        character.OnCharacterTurnEnded += OnCharacterTurnEnded;

    }

    public void OnCharacterTurnEnded(BaseCharacter character)
    {
        characterTurnData.SetElementStatus(character, false);
        CheckAvailableMoves();
    }

    private void CheckAvailableMoves()
    {
        var res = characters.Count(c => !characterTurnData.GetElementStatus(c));
        if (res == characters.Count)
        {
            EndTurn();
        }
    }

    public bool CanMove(BaseCharacter character)
    {
        return isActiveTurn && characterTurnData.GetElementStatus(character);
    }

    public void Reset()
    {
        characterTurnData.Reset();
    }

    private void EndTurn()
    {
        OnTurnEnded?.Invoke(this);
    }
}
