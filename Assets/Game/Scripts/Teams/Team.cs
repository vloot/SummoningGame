using System.Collections.Generic;
using System.Linq;

public class Team
{
    public List<BaseCharacter> Characters { get; private set; }
    private TurnData<BaseCharacter> characterTurnData;

    public TeamSide Side { get; private set; }
    public bool ControlledByPlayer { get; private set; }

    public bool isActiveTurn;

    // events
    public delegate void TurnDelegate(Team t);
    public TurnDelegate OnTurnStarted;
    public TurnDelegate OnTurnEnded;

    public Team(TeamSide side, bool controlledByPlayer)
    {
        Side = side;
        ControlledByPlayer = controlledByPlayer;

        Characters = new List<BaseCharacter>();
        // true if charactewr can move, false otherwise
        characterTurnData = new TurnData<BaseCharacter>();
    }

    public void AddCharacter(BaseCharacter character)
    {
        Characters.Add(character);
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
        var res = Characters.Count(c => !characterTurnData.GetElementStatus(c));
        if (res == Characters.Count)
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

    public void StartTurn()
    {
        isActiveTurn = true;
        OnTurnStarted?.Invoke(this);
    }

    private void EndTurn()
    {
        OnTurnEnded?.Invoke(this);
    }
}
