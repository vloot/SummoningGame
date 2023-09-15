using UnityEngine;
using System.Collections.Generic;

public class TeamController : MonoBehaviour
{
    private List<Team> teams;
    private TurnData<Team> teamTurnData;

    private int currentTeamIndex;

    private void Awake()
    {
        teams = new List<Team>();
        teamTurnData = new TurnData<Team>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ProcessMove();
        }
    }

    public void AddTeam(Team team)
    {
        teams.Add(team);
        teamTurnData.AddElement(team);
        team.OnTurnEnded += OnTurnEnded;
        ConsoleLogger.Log("Team added");
    }

    public void ProcessMove()
    {
        ConsoleLogger.Log("ProcessMove");

        if (!teamTurnData.GetElementStatus(teams[currentTeamIndex]))
        {
            // current team has no moves left
            ConsoleLogger.Log("ProcessMove (if) " + teams[currentTeamIndex] + " turn");
            FinalizeCurrentTeam();
        }

        TeamTurn(teams[currentTeamIndex]);
    }

    private void FinalizeCurrentTeam()
    {
        var team = teams[currentTeamIndex];
        team.isActiveTurn = false;
        ConsoleLogger.Log("Finalize " + team.Side + " turn");

        // increment index to move to the next team
        currentTeamIndex++;
        if (currentTeamIndex >= teams.Count)
        {
            // TODO new round, reset all teams
            currentTeamIndex = 0;
            ResetTeams(team);
        }
    }

    private void TeamTurn(Team team)
    {
        ConsoleLogger.Log("Team " + team.Side + " turn");
        team.StartTurn();
    }

    private void ResetTeams(Team t)
    {
        // reset all teams, team turn data
        foreach (var item in teams)
        {
            item.Reset();
            teamTurnData.SetElementStatus(item, true);
        }
    }

    private void OnTurnEnded(Team t)
    {
        ConsoleLogger.Log("OnTurnEnded");
        teamTurnData.SetElementStatus(t, false);
        ProcessMove();
    }

    public List<Team> GetTeams(TeamSide teamSide)
    {
        var requestedTeams = new List<Team>();

        foreach (var item in teams)
        {
            if (item.Side == teamSide)
            {
                requestedTeams.Add(item);
            }
        }

        return requestedTeams;
    }
}