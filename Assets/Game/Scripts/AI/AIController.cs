using System;
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
    private Pathfinder _pathfinder;

    private void Awake()
    {
        _tileDistance = new ManhattanTileDistance();
    }

    private void Start()
    {
        _pathfinder = componentInit.pathfinder;
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
                return;
            }
        }
    }

    public void TakeTurn(BaseCharacter character)
    {
        var characters = GetEnemyCharacters();

        if (characters.Count == 0)
        {
            // No characters left, end turn?
            EndTurn();
        }

        BasePathConfig movementConfig = new MovementPathConfig();

        foreach (var targetCharacter in characters)
        {
            ConsoleLogger.Log("AI: " + character.gameObject.name + " is checking " + targetCharacter.gameObject.name);

            if (_tileDistance.GetDistance(character.tile, targetCharacter.tile) <= character.characterStats.attackRange)
            {
                // attack character
                ConsoleLogger.Log("AI: " + character.gameObject.name + " is attacking " + targetCharacter.gameObject.name);
                AttackCharacter(character, targetCharacter);
                return;
            }
            else
            {
                ConsoleLogger.Log("AI: " + character.gameObject.name + " target " + targetCharacter.gameObject.name + " is too far (" + _tileDistance.GetDistance(character.tile, targetCharacter.tile) + " tiles away)");
            }

            // try to move towards the character, and then attack
            var characterMove = CalculateMoveToCharacter(character, targetCharacter, movementConfig);
            if (!characterMove.canMove) continue;

            ConsoleLogger.Log("AI: " + character.gameObject.name + " is moving " + targetCharacter.gameObject.name);
            componentInit.inputController.SimulateClick(characterMove.tile);
            componentInit.CommandsDict[CharacterAction.Move].ExecuteCommand(character, () => AttackCharacter(character, targetCharacter));

            // TODO handle last case where no character can be attacked or moved to
        }
    }

    public List<BaseCharacter> GetEnemyCharacters()
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

        return characters.OrderBy(c => c.characterVitals.health).ToList();
    }

    private AICharacterMove CalculateMoveToCharacter(BaseCharacter character, BaseCharacter enemyCharacter, BasePathConfig pathConfig)
    {
        var neighborTiles = _pathfinder.GetNeighbours(enemyCharacter.tile, pathConfig);
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

    private void AttackCharacter(BaseCharacter character, BaseCharacter targetCharacter)
    {
        componentInit.inputController.SimulateClick(targetCharacter);
        componentInit.CommandsDict[CharacterAction.Attack].ExecuteCommand(character);
    }

    private void EndTurn()
    {
        throw new NotImplementedException();
    }
}
