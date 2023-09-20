using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] public Team team;
    [SerializeField] private TeamSide side;

    [SerializeField] private ComponentInit componentInit;
    [SerializeField] private TeamController teamController;

    private ITileDistance _tileDistance;

    // AI actions
    private AIAttack attackAction;
    private AIMove movementAction;

    private void Awake()
    {
        _tileDistance = new ManhattanTileDistance();
    }

    private void Start()
    {
        attackAction = new AIAttack(componentInit.CommandsDict[CharacterAction.Attack], componentInit.inputController);
        movementAction = new AIMove(componentInit.CommandsDict[CharacterAction.Move], componentInit.pathfinder, componentInit.inputController);
    }

    public void InitAI(Team team)
    {
        this.team = team;
        team.OnTurnStarted += OnTurnStarted;
    }

    private void OnTurnStarted(Team team)
    {
        ConsoleLogger.Log("AI: Turn started");
        foreach (var character in team.Characters)
        {
            if (team.CanMove(character))
            {
                ConsoleLogger.Log("AI: " + character.gameObject.name + " is taking a turn");
                TakeTurn(character);
                EndTurn(character);
            }
        }
    }

    public void TakeTurn(BaseCharacter character)
    {
        // Sort the characters by health
        var characters = GetCharacters().OrderBy(c => c.characterVitals.health).ToList();

        // if no characters left, end turn?
        if (characters.Count == 0) return;

        foreach (var targetCharacter in characters)
        {
            ConsoleLogger.Log("AI: " + character.gameObject.name + " is checking " + targetCharacter.gameObject.name);

            // check if any character in range can be attacked
            if (attackAction.TryPerformAction(character, targetCharacter))
            {
                ConsoleLogger.Log("AI: " + character.gameObject.name + " is attacking " + targetCharacter.gameObject.name);
                return;
            }

            // check if it is possible to move to any characters in racge and then attack
            if (movementAction.TryPerformAction(character, targetCharacter, () => attackAction.TryPerformAction(character, targetCharacter)))
            {
                ConsoleLogger.Log("AI: " + character.gameObject.name + " is moving towards and attacking " + targetCharacter.gameObject.name);
                return;
            }
        }

        // no characters in range, try moving closer
        ConsoleLogger.Log("AI: " + character.gameObject.name + " - no characters in range, try moving closer");
        characters = GetCharacters().OrderBy(c => _tileDistance.GetDistance(character.tile, c.tile)).ToList(); // sort the characters by Manhattan distance

        foreach (var targetCharacter in characters)
        {
            if (movementAction.TryPerformAction(character, targetCharacter, depth: 2)) // FIXME remove hardcoded depth value
            {
                ConsoleLogger.Log("AI: " + character.gameObject.name + " is moving closer to " + targetCharacter.gameObject.name);
                return;
            }
        }

        // Can't move closer to any character
        ConsoleLogger.Log("AI: " + character.gameObject.name + " - no path available");
    }

    public List<BaseCharacter> GetCharacters()
    {
        var teams = teamController.GetTeams(TeamSide.Player);
        var characters = new List<BaseCharacter>();

        foreach (var team in teams)
        {
            foreach (var character in team.Characters)
            {
                characters.Add(character);
            }
        }

        return characters;
    }

    private void EndTurn(BaseCharacter character)
    {
        character.EndTurn();
    }
}
